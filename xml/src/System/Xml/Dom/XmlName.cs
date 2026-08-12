// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace dotnet.xml
{
    using System.Text;
    using System.Diagnostics;

    internal class XmlName
    {
        private string _prefix;
        private string _localName;
        private string _ns;
        private string _name;
        private int _hashCode;
        internal XmlDocument ownerDoc;
        internal XmlName next;

        public static XmlName Create(string prefix, string localName, string ns, int hashCode, XmlDocument ownerDoc, XmlName next)
        {
            return new XmlName(prefix, localName, ns, hashCode, ownerDoc, next);
        }

        internal XmlName(string prefix, string localName, string ns, int hashCode, XmlDocument ownerDoc, XmlName next)
        {
            _prefix = prefix;
            _localName = localName;
            _ns = ns;
            _name = null;
            _hashCode = hashCode;
            this.ownerDoc = ownerDoc;
            this.next = next;
        }

        public string LocalName
        {
            get
            {
                return _localName;
            }
        }

        public string NamespaceURI
        {
            get
            {
                return _ns;
            }
        }

        public string Prefix
        {
            get
            {
                return _prefix;
            }
        }

        public int HashCode
        {
            get
            {
                return _hashCode;
            }
        }

        public XmlDocument OwnerDocument
        {
            get
            {
                return ownerDoc;
            }
        }

        public string Name
        {
            get
            {
                if (_name == null)
                {
                    Debug.Assert(_prefix != null);
                    if (_prefix.Length > 0)
                    {
                        if (_localName.Length > 0)
                        {
                            string n = string.Concat(_prefix, ":", _localName);
                            lock (ownerDoc.NameTable)
                            {
                                if (_name == null)
                                {
                                    _name = ownerDoc.NameTable.Add(n);
                                }
                            }
                        }
                        else
                        {
                            _name = _prefix;
                        }
                    }
                    else
                    {
                        _name = _localName;
                    }
                    Debug.Assert(Ref.Equal(_name, ownerDoc.NameTable.Get(_name)));
                }
                return _name;
            }
        }

        public static int GetHashCode(string name)
        {
            int hashCode = 0;
            if (name != null)
            {
                unchecked
                {
                    for (int i = name.Length - 1; i >= 0; i--)
                    {
                        char ch = name[i];
                        if (ch == ':') break;
                        hashCode += (hashCode << 7) ^ ch;
                    }
                    hashCode -= hashCode >> 17;
                    hashCode -= hashCode >> 11;
                    hashCode -= hashCode >> 5;
                }
            }
            return hashCode;
        }
    }
}
