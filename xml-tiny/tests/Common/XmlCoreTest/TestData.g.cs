// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace XmlCoreTest.Common
{
    public static partial class FilePathUtil
    {
        private static Dictionary<string, byte[]> s_resourceCache = null;
        private static readonly object s_resourceLock = new object();

        private static void LoadResourcesIfNotYet()
        {
            lock (s_resourceLock)
            {
                if (s_resourceCache == null)
                {
                    s_resourceCache = new Dictionary<string, byte[]>();
                    Assembly assembly = typeof(FilePathUtil).GetTypeInfo().Assembly;
                    string[] resourceNames = assembly.GetManifestResourceNames();
                    foreach (string fullName in resourceNames)
                    {
                        if (!fullName.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                            continue;
                        string withoutExt = fullName.Substring(0, fullName.Length - 4);
                        int lastDot = withoutExt.LastIndexOf('.');
                        string key = lastDot >= 0 ? withoutExt.Substring(lastDot + 1) : withoutExt;
                        using (Stream stream = assembly.GetManifestResourceStream(fullName))
                        {
                            if (stream != null)
                            {
                                byte[] buffer = new byte[stream.Length];
                                stream.Read(buffer, 0, buffer.Length);
                                s_resourceCache[key] = buffer;
                            }
                        }
                    }
                }
            }
        }

        private static byte[] GetResourceBytes(string resourceName)
        {
            LoadResourcesIfNotYet();
            byte[] result;
            if (!s_resourceCache.TryGetValue(resourceName, out result))
                throw new FileNotFoundException("Embedded resource not found: " + resourceName);
            return result;
        }

        static IEnumerable<Tuple<string, byte[]>> GetDataFiles()
        {
            yield return Tuple.Create(@"master_xml.xml", GetResourceBytes("s_file0"));
            yield return Tuple.Create(@"ms_xmltest.xml", GetResourceBytes("s_file1"));
            yield return Tuple.Create(@"ReaderCreateSpec.xml", GetResourceBytes("s_file2"));
            yield return Tuple.Create(@"WriterCreateSpec.xml", GetResourceBytes("s_file3"));
            yield return Tuple.Create(@"xlinq_master_xml.xml", GetResourceBytes("s_file4"));
            yield return Tuple.Create(Path.Combine("StandardTests", "XML10", "ms_xml", "vs084.xml"), GetResourceBytes("s_file6"));
            yield return Tuple.Create(Path.Combine("StandardTests", "XML10", "xmlconf", "xmltest", "not-wf", "sa", "001.xml"), GetResourceBytes("s_file7"));
            yield return Tuple.Create(Path.Combine("StandardTests", "XML10", "xmlconf", "xmltest", "not-wf", "sa", "002.xml"), GetResourceBytes("s_file8"));
            yield return Tuple.Create(Path.Combine("StandardTests", "XML10", "xmlconf", "xmltest", "not-wf", "sa", "003.xml"), GetResourceBytes("s_file9"));
            yield return Tuple.Create(Path.Combine("StandardTests", "XML10", "xmlconf", "xmltest", "valid", "001.xml"), GetResourceBytes("s_file10"));
            yield return Tuple.Create(Path.Combine("StandardTests", "XML10", "xmlconf", "xmltest", "valid", "002.xml"), GetResourceBytes("s_file11"));
            yield return Tuple.Create(Path.Combine("StandardTests", "XML10", "xmlconf", "xmltest", "valid", "008.xml"), GetResourceBytes("s_file12"));
            yield return Tuple.Create(Path.Combine("TestData", "RWFactory", "utf-16-dtd.xml"), GetResourceBytes("s_file13"));
            yield return Tuple.Create(Path.Combine("TestData", "RWFactory", "utf-16-fragment.xml"), GetResourceBytes("s_file14"));
            yield return Tuple.Create(Path.Combine("TestData", "RWFactory", "utf-16.xml"), GetResourceBytes("s_file15"));
            yield return Tuple.Create(Path.Combine("TestData", "RWFactory", "utf-16be-fragment.xml"), GetResourceBytes("s_file16"));
            yield return Tuple.Create(Path.Combine("TestData", "RWFactory", "utf-16be.xml"), GetResourceBytes("s_file17"));
            yield return Tuple.Create(Path.Combine("TestData", "RWFactory", "utf-8-fragment.xml"), GetResourceBytes("s_file18"));
            yield return Tuple.Create(Path.Combine("TestData", "RWFactory", "utf-8.xml"), GetResourceBytes("s_file19"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Attributes.xml"), GetResourceBytes("s_file20"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Books.xml"), GetResourceBytes("s_file21"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "config.xml"), GetResourceBytes("s_file22"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "NoExternals.xml"), GetResourceBytes("s_file23"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Note.xml"), GetResourceBytes("s_file24"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Word.xml"), GetResourceBytes("s_file25"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "DuplicateNamespaces", "attributes.xml"), GetResourceBytes("s_file26"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "DuplicateNamespaces", "Simple.xml"), GetResourceBytes("s_file27"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "IXmlLineInfoTests", "company-data.xml"), GetResourceBytes("s_file28"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "config.xml"), GetResourceBytes("s_file29"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "first.xml"), GetResourceBytes("s_file30"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "namespaces.xml"), GetResourceBytes("s_file31"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular-element_epilog.xml"), GetResourceBytes("s_file32"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular-element_NO_prologs_middle.xml"), GetResourceBytes("s_file33"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular-element_prologs.xml"), GetResourceBytes("s_file34"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular-element_prologs_middle.xml"), GetResourceBytes("s_file35"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular-element_prologs_middle_epilog.xml"), GetResourceBytes("s_file36"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular-prolog_in_missing_data1.xml"), GetResourceBytes("s_file37"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular-prolog_in_missing_data2.xml"), GetResourceBytes("s_file38"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular-prolog_in_missing_data3.xml"), GetResourceBytes("s_file39"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular_clean.xml"), GetResourceBytes("s_file40"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "regular_missing_depth1.xml"), GetResourceBytes("s_file41"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "second.xml"), GetResourceBytes("s_file42"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "streamingTests.config"), GetResourceBytes("s_file43"));
            yield return Tuple.Create(Path.Combine("TestData", "XLinq", "Streaming", "IXmlLineInfoTests", "company-data.xml"), GetResourceBytes("s_file44"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "API", "Generic.xml"), GetResourceBytes("s_file45"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "100attr.xml"), GetResourceBytes("s_file46"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "1_GB18030.xml"), GetResourceBytes("s_file47"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "3_UTF32.xml"), GetResourceBytes("s_file48"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Attr200Valid.xml"), GetResourceBytes("s_file49"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Attr200WithNS.xml"), GetResourceBytes("s_file50"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Attr201Invalid.xml"), GetResourceBytes("s_file51"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Attr201WithNS.xml"), GetResourceBytes("s_file52"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "AttributesLargeNumber.xml"), GetResourceBytes("s_file53"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "bad.xml"), GetResourceBytes("s_file54"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug26771.xml"), GetResourceBytes("s_file55"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug297091.xsl"), GetResourceBytes("s_file56"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug411697.xml"), GetResourceBytes("s_file57"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug424573.xml"), GetResourceBytes("s_file58"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug61321.xml"), GetResourceBytes("s_file59"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug615675.xml"), GetResourceBytes("s_file60"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug68766.xml"), GetResourceBytes("s_file61"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug81489.xml"), GetResourceBytes("s_file62"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug86503.txt"), GetResourceBytes("s_file63"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug92020c.xml"), GetResourceBytes("s_file64"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug95253.xml"), GetResourceBytes("s_file65"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "Bug99148.xml"), GetResourceBytes("s_file66"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "bug_57723.xml"), GetResourceBytes("s_file67"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "bug_62146.xml"), GetResourceBytes("s_file68"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "bug_65660a.xml"), GetResourceBytes("s_file69"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "bug_65660b.xml"), GetResourceBytes("s_file70"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "bug_70237.xml"), GetResourceBytes("s_file71"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "charset01.xml"), GetResourceBytes("s_file72"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "charset02.xml"), GetResourceBytes("s_file73"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "charset03.xml"), GetResourceBytes("s_file74"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "file_23.xml"), GetResourceBytes("s_file75"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "hugeattributes.xml"), GetResourceBytes("s_file76"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "invalid-ucs4.xml"), GetResourceBytes("s_file77"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "invalid-ucs4_1234.xml"), GetResourceBytes("s_file78"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "invalid-ucs4_2143.xml"), GetResourceBytes("s_file79"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "invalid-ucs4_3412.xml"), GetResourceBytes("s_file80"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "LineNumber.xml"), GetResourceBytes("s_file81"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "riversrss.xml"), GetResourceBytes("s_file82"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "skip307543.xml"), GetResourceBytes("s_file83"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "valid-ucs4.xml"), GetResourceBytes("s_file84"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "valid-ucs4_1234.xml"), GetResourceBytes("s_file85"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "valid-ucs4_2143.xml"), GetResourceBytes("s_file86"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "Common", "valid-ucs4_3412.xml"), GetResourceBytes("s_file87"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "DateTime", "euc-jp.xml"), GetResourceBytes("s_file88"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "DateTime", "extpe.xml"), GetResourceBytes("s_file89"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "DateTime", "iso-2022-jp.xml"), GetResourceBytes("s_file90"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "DateTime", "ix024.xml"), GetResourceBytes("s_file91"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "DateTime", "vx024.xml"), GetResourceBytes("s_file92"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "DateTime", "vx026.xml"), GetResourceBytes("s_file93"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlReader", "DateTime", "vx030.xml"), GetResourceBytes("s_file94"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "100Elements.txt"), GetResourceBytes("s_file95"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "100ElementsIndent.txt"), GetResourceBytes("s_file96"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "100FullEndElements.txt"), GetResourceBytes("s_file97"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "100Nodes.txt"), GetResourceBytes("s_file98"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "bug364698.xml"), GetResourceBytes("s_file99"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "NewLineOnAttributes3.txt"), GetResourceBytes("s_file100"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "OneHundredAttributes.xml"), GetResourceBytes("s_file101"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "Simple.xml"), GetResourceBytes("s_file102"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "whitespace1.txt"), GetResourceBytes("s_file103"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "whitespace2.txt"), GetResourceBytes("s_file104"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "whitespace3.txt"), GetResourceBytes("s_file105"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "writeStringWhiespaceInAttr.txt"), GetResourceBytes("s_file106"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "writeStringWhiespaceInElem.txt"), GetResourceBytes("s_file107"));
            yield return Tuple.Create(Path.Combine("TestData", "XmlWriter2", "XmlReader.xml"), GetResourceBytes("s_file108"));
        }
    }
}
