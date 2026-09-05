// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace dotnet.xml.serialization
{
    using System.Configuration;
    using System.Reflection;
    using System.Collections;
    using System.IO;
    using System;
    using System.Text;
    using dotnet.xml;
    using System.Threading;
    using System.Security;
    using dotnet.xml.serialization.configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.Versioning;
    using System.Diagnostics.CodeAnalysis;
    using System.Collections.Generic;
    using dotnet.xml.extensions;
    using System.Linq;
    using dotnet.xml.serialization;

    internal class TempAssembly
    {
        internal const string GeneratedAssemblyNamespace = "Microsoft.Xml.Serialization.GeneratedAssembly";
        private Assembly _assembly = null;
        private XmlSerializerImplementation _contract = null;
        private IDictionary _writerMethods;
        private IDictionary _readerMethods;
        private TempMethodDictionary _methods;
        private Hashtable _assemblies = new Hashtable();

        internal class TempMethod
        {
            internal MethodInfo writeMethod;
            internal MethodInfo readMethod;
            internal string name;
            internal string ns;
            internal bool isSoap;
            internal string methodKey;
        }

        private TempAssembly()
        {
        }

        internal TempAssembly(XmlMapping[] xmlMappings, Assembly assembly, XmlSerializerImplementation contract)
        {
            _assembly = assembly;
            InitAssemblyMethods(xmlMappings);
            _contract = contract;
        }

        internal XmlSerializerImplementation Contract
        {
            get
            {
                if (_contract == null)
                {
                    _contract = (XmlSerializerImplementation)Activator.CreateInstance(GetTypeFromAssembly(_assembly, "XmlSerializerContract"));
                }
                return _contract;
            }
        }

        internal void InitAssemblyMethods(XmlMapping[] xmlMappings)
        {
            _methods = new TempMethodDictionary();
            for (int i = 0; i < xmlMappings.Length; i++)
            {
                TempMethod method = new TempMethod();
                method.isSoap = xmlMappings[i].IsSoap;
                method.methodKey = xmlMappings[i].Key;
                XmlTypeMapping xmlTypeMapping = xmlMappings[i] as XmlTypeMapping;
                if (xmlTypeMapping != null)
                {
                    method.name = xmlTypeMapping.ElementName;
                    method.ns = xmlTypeMapping.Namespace;
                }
                _methods.Add(xmlMappings[i].Key, method);
            }
        }

        /// <devdoc>
        ///    <para>
        ///    Attempts to load pre-generated serialization assembly.
        ///    First check for the [XmlSerializerAssembly] attribute
        ///    </para>
        /// </devdoc>
        // SxS: This method does not take any resource name and does not expose any resources to the caller.
        // It's OK to suppress the SxS warning.
        internal static Assembly LoadGeneratedAssembly(Type type, string defaultNamespace, out XmlSerializerImplementation contract)
        {
            Assembly serializer = null;
            contract = null;
            string serializerName = null;

            // check to see if we loading explicit pre-generated assembly
            object[] attrs = type.GetCustomAttributes(typeof(dotnet.xml.serialization.XmlSerializerAssemblyAttribute), false);
            if (attrs.Length == 0)
            {
                // Guess serializer name: if parent assembly signed use strong name 
                AssemblyName name = type.Assembly.GetName();
                serializerName = Compiler.GetTempAssemblyName(name, defaultNamespace);
                // use strong name 
                name.Name = serializerName;
                name.CodeBase = null;
                name.CultureInfo = CultureInfo.InvariantCulture;

                string serializerPath = null;

                try
                {
                    if (!string.IsNullOrEmpty(type.Assembly.Location))
                    {
                        serializerPath = Path.Combine(Path.GetDirectoryName(type.Assembly.Location), serializerName + ".dll");
                    }

                    if ((string.IsNullOrEmpty(serializerPath) || !File.Exists(serializerPath)) && !string.IsNullOrEmpty(Assembly.GetEntryAssembly().Location))
                    {
                        serializerPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), serializerName + ".dll");
                    }

                    if (!string.IsNullOrEmpty(serializerPath))
                    {
                        serializer = Assembly.LoadFile(serializerPath);
                    }
                }
                catch (Exception e)
                {
                    if (e is ThreadAbortException || e is StackOverflowException || e is OutOfMemoryException)
                    {
                        throw;
                    }
                    byte[] token = name.GetPublicKeyToken();
                    if (token != null && token.Length > 0)
                    {
                        // the parent assembly was signed, so do not try to LoadWithPartialName
                        return null;
                    }
                }

                if (serializer == null)
                {
                    if (XmlSerializer.Mode == SerializationMode.PreGenOnly)
                    {
                        throw new Exception(SR.Format(SR.FailLoadAssemblyUnderPregenMode, serializerName));
                    }

                    return null;
                }
            }
            else
            {
                dotnet.xml.serialization.XmlSerializerAssemblyAttribute assemblyAttribute = (dotnet.xml.serialization.XmlSerializerAssemblyAttribute)attrs[0];
                if (assemblyAttribute.AssemblyName != null && assemblyAttribute.CodeBase != null)
                    throw new InvalidOperationException(SR.Format(SR.XmlPregenInvalidXmlSerializerAssemblyAttribute, "AssemblyName", "CodeBase"));

                // found XmlSerializerAssemblyAttribute attribute, it should have all needed information to load the pre-generated serializer
                if (assemblyAttribute.AssemblyName != null)
                {
                    serializerName = assemblyAttribute.AssemblyName;
#pragma warning disable 618
                    serializer = Assembly.LoadWithPartialName(serializerName);
#pragma warning restore 618
                }
                else if (assemblyAttribute.CodeBase != null && assemblyAttribute.CodeBase.Length > 0)
                {
                    serializerName = assemblyAttribute.CodeBase;
                    serializer = Assembly.LoadFrom(serializerName);
                }
                else
                {
                    serializerName = type.Assembly.FullName;
                    serializer = type.Assembly;
                }
                if (serializer == null)
                {
                    throw new FileNotFoundException(null, serializerName);
                }
            }
            Type contractType = GetTypeFromAssembly(serializer, "XmlSerializerContract");
            contract = (XmlSerializerImplementation)Activator.CreateInstance(contractType);
            if (contract.CanSerialize(type))
                return serializer;

            return null;
        }


        private static MethodInfo GetMethodFromType(Type type, string methodName)
        {
            MethodInfo method = type.GetMethod(methodName);
            if (method != null)
                return method;

            // Not support pregen.  Workaround SecurityCritical required for assembly.CodeBase api.
            MissingMethodException missingMethod = new MissingMethodException(type.FullName + "::" + methodName);
            throw missingMethod;
        }

        internal static Type GetTypeFromAssembly(Assembly assembly, string typeName)
        {
            typeName = GeneratedAssemblyNamespace + "." + typeName;
            Type type = assembly.GetType(typeName);
            if (type == null)
                throw new InvalidOperationException(SR.Format(SR.XmlMissingType, typeName, assembly.FullName));
            return type;
        }

        internal bool CanRead(XmlMapping mapping, XmlReader xmlReader)
        {
            if (mapping == null)
                return false;

            if (mapping.Accessor.Any)
            {
                return true;
            }
            TempMethod method = _methods[mapping.Key];
            return xmlReader.IsStartElement(method.name, method.ns);
        }

        private string ValidateEncodingStyle(string encodingStyle, string methodKey)
        {
            if (encodingStyle != null && encodingStyle.Length > 0)
            {
                if (_methods[methodKey].isSoap)
                {
                    if (encodingStyle != Soap.Encoding && encodingStyle != Soap12.Encoding)
                    {
                        throw new InvalidOperationException(SR.Format(SR.XmlInvalidEncoding3, encodingStyle, Soap.Encoding, Soap12.Encoding));
                    }
                }
                else
                {
                    throw new InvalidOperationException(SR.Format(SR.XmlInvalidEncodingNotEncoded1, encodingStyle));
                }
            }
            else
            {
                if (_methods[methodKey].isSoap)
                {
                    encodingStyle = Soap.Encoding;
                }
            }
            return encodingStyle;
        }

        internal object InvokeReader(XmlMapping mapping, XmlReader xmlReader, XmlDeserializationEvents events, string encodingStyle)
        {
            XmlSerializationReader reader = null;
            try
            {
                encodingStyle = ValidateEncodingStyle(encodingStyle, mapping.Key);
                reader = Contract.Reader;
                reader.Init(xmlReader, events, encodingStyle, this);
                if (_methods[mapping.Key].readMethod == null)
                {
                    if (_readerMethods == null)
                    {
                        _readerMethods = Contract.ReadMethods;
                    }
                    string methodName = (string)_readerMethods[mapping.Key];
                    if (methodName == null)
                    {
                        throw new InvalidOperationException(SR.Format(SR.XmlNotSerializable, mapping.Accessor.Name));
                    }
                    _methods[mapping.Key].readMethod = GetMethodFromType(reader.GetType(), methodName);
                }
                return _methods[mapping.Key].readMethod.Invoke(reader, Array.Empty<object>());
            }
            catch (SecurityException e)
            {
                throw new InvalidOperationException(SR.XmlNoPartialTrust, e);
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

        internal void InvokeWriter(XmlMapping mapping, XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces, string encodingStyle, string id)
        {
            XmlSerializationWriter writer = null;
            try
            {
                encodingStyle = ValidateEncodingStyle(encodingStyle, mapping.Key);
                writer = Contract.Writer;
                writer.Init(xmlWriter, namespaces, encodingStyle, id, this);
                if (_methods[mapping.Key].writeMethod == null)
                {
                    if (_writerMethods == null)
                    {
                        _writerMethods = Contract.WriteMethods;
                    }
                    string methodName = (string)_writerMethods[mapping.Key];
                    if (methodName == null)
                    {
                        throw new InvalidOperationException(SR.Format(SR.XmlNotSerializable, mapping.Accessor.Name));
                    }
                    _methods[mapping.Key].writeMethod = GetMethodFromType(writer.GetType(), methodName);
                }
                _methods[mapping.Key].writeMethod.Invoke(writer, new object[] { o });
            }
            catch (SecurityException e)
            {
                throw new InvalidOperationException(SR.XmlNoPartialTrust, e);
            }
            finally
            {
                if (writer != null)
                    writer.Dispose();
            }
        }

        internal sealed class TempMethodDictionary : Dictionary<string, TempMethod>
        {
        }
    }

    internal class TempAssemblyCacheKey
    {
        private string _ns;
        private object _type;

        internal TempAssemblyCacheKey(string ns, object type)
        {
            _type = type;
            _ns = ns;
        }

        public override bool Equals(object o)
        {
            TempAssemblyCacheKey key = o as TempAssemblyCacheKey;
            if (key == null)
                return false;
            return (key._type == _type && key._ns == _ns);
        }

        public override int GetHashCode()
        {
            return ((_ns != null ? _ns.GetHashCode() : 0) ^ (_type != null ? _type.GetHashCode() : 0));
        }
    }

    internal class TempAssemblyCache
    {
        private Dictionary<TempAssemblyCacheKey, TempAssembly> _cache = new Dictionary<TempAssemblyCacheKey, TempAssembly>();

        internal TempAssembly this[string ns, object o]
        {
            get
            {
                TempAssembly tempAssembly;
                _cache.TryGetValue(new TempAssemblyCacheKey(ns, o), out tempAssembly);
                return tempAssembly;
            }
        }

        internal void Add(string ns, object o, TempAssembly assembly)
        {
            TempAssemblyCacheKey key = new TempAssemblyCacheKey(ns, o);
            lock (this)
            {
                TempAssembly tempAssembly;
                if (_cache.TryGetValue(key, out tempAssembly) && tempAssembly == assembly)
                    return;
                _cache = new Dictionary<TempAssemblyCacheKey, TempAssembly>(_cache); // clone
                _cache[key] = assembly;
            }
        }
    }

    internal static class ThisAssembly
    {
        internal const string Version = "1.0.0.0";
        internal const string InformationalVersion = "1.0.0.0";
    }
}

