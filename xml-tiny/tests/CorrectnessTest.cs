using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Linq;

using Cx = dotnet.xml;
using CxXsl = dotnet.xml.Xsl;
using CxXPath = dotnet.xml.xpath;
using CxSchema = dotnet.xml.schema;

/// <summary>
/// 正确性验证测试
/// Part 1: 使用框架 XML 类型验证核心功能正确性（基线）
/// Part 2: 验证迁移后的自定义库 API 表面完整
/// Part 3: 验证自定义库功能（XSLT / XPath / Schema）
/// </summary>
class CorrectnessTest
{
    static int _passed = 0;
    static int _failed = 0;

    static int Main(string[] args)
    {
        Console.WriteLine("=== dotnet.xml 迁移后正确性验证测试 ===\n");

        Console.WriteLine("--- Part 1: 核心功能基线测试 (框架 XML 类型) ---\n");

        Test("XmlReader_ParseElements", Test_XmlReader_ParseElements);
        Test("XmlReader_ParseAttributes", Test_XmlReader_ParseAttributes);
        Test("XmlReader_ParseNamespaces", Test_XmlReader_ParseNamespaces);
        Test("XmlReader_ParseCDATA", Test_XmlReader_ParseCDATA);
        Test("XmlReader_ParseComments", Test_XmlReader_ParseComments);
        Test("XmlReader_ReadContentAs", Test_XmlReader_ReadContentAs);
        Test("XmlReader_ReadInnerOuterXml", Test_XmlReader_ReadInnerOuterXml);

        Test("XmlWriter_WriteElements", Test_XmlWriter_WriteElements);
        Test("XmlWriter_WriteAttributes", Test_XmlWriter_WriteAttributes);
        Test("XmlWriter_WriteNamespaces", Test_XmlWriter_WriteNamespaces);
        Test("XmlWriter_WriteCDATA", Test_XmlWriter_WriteCDATA);
        Test("XmlWriter_WriteComments", Test_XmlWriter_WriteComments);
        Test("XmlWriter_WriteNode_Reader", Test_XmlWriter_WriteNode_Reader);

        Test("XmlDocument_CreateAndSave", Test_XmlDocument_CreateAndSave);
        Test("XmlDocument_LoadAndQuery", Test_XmlDocument_LoadAndQuery);
        Test("XmlDocument_ModifyNodes", Test_XmlDocument_ModifyNodes);
        Test("XmlDocument_InnerXml", Test_XmlDocument_InnerXml);
        Test("XmlDocument_NamespaceHandling", Test_XmlDocument_NamespaceHandling);
        Test("XmlDocument_ImportNode", Test_XmlDocument_ImportNode);

        Test("XmlReader_DtdProcessing_Parse", Test_XmlReader_DtdProcessing_Parse);
        Test("XmlConvert_EncodeDecodeName", Test_XmlConvert_EncodeDecodeName);
        Test("XmlConvert_TypeConversions", Test_XmlConvert_TypeConversions);
        Test("Integration_ReaderToWriter", Test_Integration_ReaderToWriter);
        Test("Integration_RoundTrip", Test_Integration_RoundTrip);

        Console.WriteLine("\n--- Part 2: 自定义库 API 表面验证 ---\n");

        var asm = typeof(Cx.XmlReader).Assembly;

        Test("Assembly_Loads", () => AssertTrue(asm != null, "Assembly should load"));

        Test("CoreTypes_Exist", () => {
            var coreTypes = new[] {
                "dotnet.xml.XmlReader", "dotnet.xml.XmlWriter",
                "dotnet.xml.XmlDocument", "dotnet.xml.XmlElement",
                "dotnet.xml.XmlAttribute", "dotnet.xml.XmlNode",
                "dotnet.xml.XmlTextReader", "dotnet.xml.XmlTextWriter",
                "dotnet.xml.XmlReaderSettings", "dotnet.xml.XmlWriterSettings",
                "dotnet.xml.XmlConvert", "dotnet.xml.XmlNamespaceManager",
                "dotnet.xml.XmlResolver", "dotnet.xml.XmlUrlResolver",
            };
            foreach (var typeName in coreTypes)
            {
                var t = asm.GetType(typeName);
                AssertTrue(t != null, $"{typeName} should exist in custom assembly");
            }
        });

        Test("XPathTypes_Exist", () => {
            var types = new[] {
                "dotnet.xml.xpath.XPathDocument",
                "dotnet.xml.xpath.XPathNavigator",
                "dotnet.xml.xpath.XPathNodeIterator",
                "dotnet.xml.xpath.XPathExpression",
            };
            foreach (var typeName in types)
            {
                var t = asm.GetType(typeName);
                AssertTrue(t != null, $"{typeName} should exist");
            }
        });

        Test("SchemaTypes_Exist", () => {
            var types = new[] {
                "dotnet.xml.schema.XmlSchema",
                "dotnet.xml.schema.XmlSchemaSet",
                "dotnet.xml.schema.XmlSchemaValidator",
                "dotnet.xml.schema.XmlSchemaValidationException",
            };
            foreach (var typeName in types)
            {
                var t = asm.GetType(typeName);
                AssertTrue(t != null, $"{typeName} should exist");
            }
        });

        Test("XsltTypes_Exist", () => {
            var types = new[] {
                "dotnet.xml.Xsl.XsltArgumentList",
                "dotnet.xml.Xsl.XslTransform",
            };
            foreach (var typeName in types)
            {
                var t = asm.GetType(typeName);
                AssertTrue(t != null, $"{typeName} should exist");
            }
        });

        Console.WriteLine("\n--- Part 3: 自定义库功能测试 ---\n");

        Test("Custom_XslTransform", Test_Custom_XslTransform);
        Test("Custom_XPathNavigator_Select", Test_Custom_XPathNavigator_Select);
        Test("Custom_XmlSchemaSet_Validate", Test_Custom_XmlSchemaSet_Validate);
        Test("Custom_XmlDocument_SelectNodes", Test_Custom_XmlDocument_SelectNodes);

        Console.WriteLine($"\n=== 测试结果: {_passed} 通过, {_failed} 失败 ===");
        return _failed > 0 ? 1 : 0;
    }

    static void Test(string name, Action action)
    {
        try
        {
            action();
            _passed++;
            Console.WriteLine($"  [PASS] {name}");
        }
        catch (Exception ex)
        {
            _failed++;
            Console.WriteLine($"  [FAIL] {name}: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"         -> {ex.InnerException.Message}");
        }
    }

    // === Part 1: 框架 XML 基线测试 ===

    static void Test_XmlReader_ParseElements()
    {
        string xml = "<root><child1>text1</child1><child2>text2</child2><empty/></root>";
        using var reader = XmlReader.Create(new StringReader(xml));
        reader.MoveToContent();
        AssertEqual("root", reader.LocalName);
        reader.Read();
        AssertEqual("child1", reader.LocalName);
        reader.Read();
        AssertEqual("text1", reader.Value);
    }

    static void Test_XmlReader_ParseAttributes()
    {
        string xml = "<root attr1='val1' attr2='val2'/>";
        using var reader = XmlReader.Create(new StringReader(xml));
        reader.MoveToContent();
        AssertEqual("val1", reader.GetAttribute("attr1"));
        AssertEqual("val2", reader.GetAttribute("attr2"));
        AssertEqual(2, reader.AttributeCount);
    }

    static void Test_XmlReader_ParseNamespaces()
    {
        string xml = "<root xmlns:ns='http://example.com'><ns:child/></root>";
        using var reader = XmlReader.Create(new StringReader(xml));
        reader.MoveToContent();
        reader.Read();
        AssertEqual("child", reader.LocalName);
        AssertEqual("http://example.com", reader.NamespaceURI);
        AssertEqual("ns", reader.Prefix);
    }

    static void Test_XmlReader_ParseCDATA()
    {
        string xml = "<root><![CDATA[<special>&content</special>]]></root>";
        using var reader = XmlReader.Create(new StringReader(xml));
        reader.MoveToContent();
        reader.Read();
        AssertEqual(XmlNodeType.CDATA, reader.NodeType);
        AssertEqual("<special>&content</special>", reader.Value);
    }

    static void Test_XmlReader_ParseComments()
    {
        string xml = "<root><!-- this is a comment --></root>";
        using var reader = XmlReader.Create(new StringReader(xml));
        reader.MoveToContent();
        reader.Read();
        AssertEqual(XmlNodeType.Comment, reader.NodeType);
        AssertEqual(" this is a comment ", reader.Value);
    }

    static void Test_XmlReader_ReadContentAs()
    {
        string xml = "<root>42</root>";
        using var reader = XmlReader.Create(new StringReader(xml));
        reader.MoveToContent();
        reader.Read();
        int val = reader.ReadContentAsInt();
        AssertEqual(42, val);
    }

    static void Test_XmlReader_ReadInnerOuterXml()
    {
        string xml = "<root><child>text</child></root>";
        using var reader = XmlReader.Create(new StringReader(xml));
        reader.MoveToContent();
        string inner = reader.ReadInnerXml();
        AssertTrue(inner.Contains("<child>text</child>"), "InnerXml should contain child");
    }

    static void Test_XmlWriter_WriteElements()
    {
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb);
        writer.WriteStartElement("root");
        writer.WriteElementString("child", "hello");
        writer.WriteEndElement();
        writer.Flush();
        string result = sb.ToString();
        AssertTrue(result.Contains("<root>"), "Should contain <root>");
        AssertTrue(result.Contains("<child>hello</child>"), "Should contain child");
    }

    static void Test_XmlWriter_WriteAttributes()
    {
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb);
        writer.WriteStartElement("root");
        writer.WriteAttributeString("attr1", "val1");
        writer.WriteAttributeString("attr2", "val2");
        writer.WriteEndElement();
        writer.Flush();
        string result = sb.ToString();
        AssertTrue(result.Contains("attr1=\"val1\""), "Should contain attr1");
        AssertTrue(result.Contains("attr2=\"val2\""), "Should contain attr2");
    }

    static void Test_XmlWriter_WriteNamespaces()
    {
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb);
        writer.WriteStartElement("ns", "root", "http://example.com");
        writer.WriteElementString("ns", "child", "http://example.com", "text");
        writer.WriteEndElement();
        writer.Flush();
        string result = sb.ToString();
        AssertTrue(result.Contains("http://example.com"), "Should contain namespace");
    }

    static void Test_XmlWriter_WriteCDATA()
    {
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb);
        writer.WriteStartElement("root");
        writer.WriteCData("<special>&content</special>");
        writer.WriteEndElement();
        writer.Flush();
        string result = sb.ToString();
        AssertTrue(result.Contains("<![CDATA[<special>&content</special>]]>"), "Should contain CDATA");
    }

    static void Test_XmlWriter_WriteComments()
    {
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb);
        writer.WriteStartElement("root");
        writer.WriteComment("a comment");
        writer.WriteEndElement();
        writer.Flush();
        string result = sb.ToString();
        AssertTrue(result.Contains("<!--a comment-->"), "Should contain comment");
    }

    static void Test_XmlWriter_WriteNode_Reader()
    {
        string xml = "<root><child attr='val'>text</child></root>";
        var sb = new StringBuilder();
        using var reader = XmlReader.Create(new StringReader(xml));
        using var writer = XmlWriter.Create(sb);
        writer.WriteNode(reader, false);
        writer.Flush();
        string result = sb.ToString();
        AssertTrue(result.Contains("attr=\"val\""), "Should preserve attributes");
        AssertTrue(result.Contains(">text<"), "Should preserve text");
    }

    static void Test_XmlDocument_CreateAndSave()
    {
        var doc = new XmlDocument();
        doc.LoadXml("<root/>");
        var elem = doc.CreateElement("child");
        elem.InnerText = "hello";
        doc.DocumentElement.AppendChild(elem);
        AssertTrue(doc.DocumentElement.InnerXml.Contains("<child>hello</child>"), "Should contain child");
    }

    static void Test_XmlDocument_LoadAndQuery()
    {
        string xml = "<root><item id='1'>first</item><item id='2'>second</item></root>";
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var items = doc.DocumentElement.GetElementsByTagName("item");
        AssertEqual(2, items.Count);
        AssertEqual("first", items[0].InnerText);
        AssertEqual("second", items[1].InnerText);
    }

    static void Test_XmlDocument_ModifyNodes()
    {
        var doc = new XmlDocument();
        doc.LoadXml("<root><a>1</a><b>2</b></root>");
        var a = doc.DocumentElement.FirstChild;
        doc.DocumentElement.RemoveChild(a);
        AssertEqual(1, doc.DocumentElement.ChildNodes.Count);
        var c = doc.CreateElement("c");
        c.InnerText = "3";
        doc.DocumentElement.AppendChild(c);
        AssertEqual(2, doc.DocumentElement.ChildNodes.Count);
    }

    static void Test_XmlDocument_InnerXml()
    {
        var doc = new XmlDocument();
        doc.LoadXml("<root><child>text</child></root>");
        AssertEqual("<child>text</child>", doc.DocumentElement.InnerXml);
        doc.DocumentElement.InnerXml = "<newChild>newText</newChild>";
        AssertEqual("newText", doc.DocumentElement.FirstChild.InnerText);
    }

    static void Test_XmlDocument_NamespaceHandling()
    {
        var doc = new XmlDocument();
        doc.LoadXml("<root xmlns:ns='http://example.com'><ns:child>text</ns:child></root>");
        var child = doc.DocumentElement.FirstChild;
        AssertEqual("child", child.LocalName);
        AssertEqual("http://example.com", child.NamespaceURI);
        AssertEqual("ns", child.Prefix);
    }

    static void Test_XmlDocument_ImportNode()
    {
        var doc1 = new XmlDocument();
        doc1.LoadXml("<root><child>text</child></root>");
        var doc2 = new XmlDocument();
        doc2.LoadXml("<root/>");
        var imported = doc2.ImportNode(doc1.DocumentElement.FirstChild, true);
        doc2.DocumentElement.AppendChild(imported);
        AssertEqual("text", doc2.DocumentElement.FirstChild.InnerText);
    }

    static void Test_XmlReader_DtdProcessing_Parse()
    {
        string xml = "<!DOCTYPE root [<!ENTITY e 'entitytext'>]><root>&e;</root>";
        var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };
        using var reader = XmlReader.Create(new StringReader(xml), settings);
        reader.MoveToContent();
        reader.Read();
        AssertEqual("entitytext", reader.Value);
    }

    static void Test_XmlConvert_EncodeDecodeName()
    {
        string encoded = XmlConvert.EncodeName("a b");
        AssertTrue(encoded != "a b", "Encoded name should differ");
        string decoded = XmlConvert.DecodeName(encoded);
        AssertEqual("a b", decoded);
    }

    static void Test_XmlConvert_TypeConversions()
    {
        AssertEqual(true, XmlConvert.ToBoolean("true"));
        AssertEqual(42, XmlConvert.ToInt32("42"));
        AssertTrue(Math.Abs(3.14 - XmlConvert.ToDouble("3.14")) < 0.001, "Double conversion");
    }

    static void Test_Integration_ReaderToWriter()
    {
        string xml = "<root><parent><child attr='value'>text</child></parent></root>";
        var sb = new StringBuilder();
        using var reader = XmlReader.Create(new StringReader(xml));
        using var writer = XmlWriter.Create(sb);
        while (reader.NodeType != XmlNodeType.Element)
            reader.Read();
        writer.WriteNode(reader, false);
        writer.Flush();
        string result = sb.ToString();
        AssertTrue(result.Contains("attr=\"value\""), "Should preserve attributes");
    }

    static void Test_Integration_RoundTrip()
    {
        var sb = new StringBuilder();
        using (var writer = XmlWriter.Create(sb))
        {
            writer.WriteStartElement("root");
            writer.WriteAttributeString("version", "1.0");
            writer.WriteElementString("item", "hello");
            writer.WriteComment("a comment");
            writer.WriteStartElement("nested");
            writer.WriteCData("<raw/>");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
        }

        using var reader = XmlReader.Create(new StringReader(sb.ToString()));
        reader.MoveToContent();
        AssertEqual("root", reader.LocalName);
        AssertEqual("1.0", reader.GetAttribute("version"));
    }

    // === Part 3: 自定义库功能测试 ===

    static void Test_Custom_XslTransform()
    {
        string xslt = @"<?xml version='1.0'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
  <xsl:output method='xml' indent='yes'/>
  <xsl:template match='/'>
    <output>
      <xsl:value-of select='/root/message'/>
    </output>
  </xsl:template>
</xsl:stylesheet>";

        string input = "<root><message>hello xslt</message></root>";

        var transform = new CxXsl.XslTransform();
        using (var xsltReader = Cx.XmlReader.Create(new StringReader(xslt)))
        {
            transform.Load(xsltReader);
        }

        var sb = new StringBuilder();
        var inputDoc = new Cx.XmlDocument();
        inputDoc.LoadXml(input);
        using (var outputWriter = Cx.XmlWriter.Create(new StringWriter(sb)))
        {
            transform.Transform(inputDoc, null, outputWriter);
        }

        string result = sb.ToString();
        AssertTrue(result.Contains("<output>"), "XSLT output should contain <output>");
        AssertTrue(result.Contains("hello xslt"), "XSLT output should contain transformed text");
    }

    static void Test_Custom_XPathNavigator_Select()
    {
        string xml = "<root><item id='1'>first</item><item id='2'>second</item></root>";
        var doc = new Cx.XmlDocument();
        doc.LoadXml(xml);
        var nav = doc.CreateNavigator();
        var iter = nav.Select("//item[@id='2']");
        AssertTrue(iter.MoveNext(), "Should find matching node");
        AssertEqual("second", iter.Current.Value);
    }

    static void Test_Custom_XmlSchemaSet_Validate()
    {
        string xsd = @"<?xml version='1.0'?>
<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>
  <xs:element name='root'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='child' type='xs:string'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

        string validXml = "<root><child>text</child></root>";
        string invalidXml = "<root><wrong>text</wrong></root>";

        var schemaSet = new CxSchema.XmlSchemaSet();
        schemaSet.Add("", Cx.XmlReader.Create(new StringReader(xsd)));

        var validDoc = new Cx.XmlDocument();
        validDoc.LoadXml(validXml);
        validDoc.Schemas = schemaSet;
        validDoc.Validate(null); // should not throw

        var invalidDoc = new Cx.XmlDocument();
        invalidDoc.LoadXml(invalidXml);
        invalidDoc.Schemas = schemaSet;
        bool threw = false;
        try
        {
            invalidDoc.Validate(null);
        }
        catch (CxSchema.XmlSchemaValidationException)
        {
            threw = true;
        }
        AssertTrue(threw, "Invalid document should throw validation exception");
    }

    static void Test_Custom_XmlDocument_SelectNodes()
    {
        string xml = "<root><item id='1'>first</item><item id='2'>second</item></root>";
        var doc = new Cx.XmlDocument();
        doc.LoadXml(xml);
        var nodes = doc.SelectNodes("//item");
        AssertEqual(2, nodes.Count);
        nodes = doc.SelectNodes("//item[@id='1']");
        AssertEqual(1, nodes.Count);
    }

    // === 辅助方法 ===

    static void AssertEqual<T>(T expected, T actual)
    {
        if (!expected.Equals(actual))
            throw new Exception($"Expected '{expected}' but got '{actual}'");
    }

    static void AssertTrue(bool condition, string message)
    {
        if (!condition)
            throw new Exception($"Assertion failed: {message}");
    }
}
