using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace Serialization.Tests
{
    public class SimpleSerializerTests
    {
        [Fact]
        public void CanSerializeAndDeserializeSimpleClass()
        {
            var person = new Person { Name = "Alice", Age = 30 };
            var serializer = new XmlSerializer(typeof(Person));
            string xml;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, person);
                xml = writer.ToString();
            }
            Assert.Contains("Alice", xml);
            Assert.Contains("30", xml);

            Person deserialized;
            using (var reader = new StringReader(xml))
            {
                deserialized = (Person)serializer.Deserialize(reader);
            }
            Assert.Equal("Alice", deserialized.Name);
            Assert.Equal(30, deserialized.Age);
        }

        [Fact]
        public void CanUseXmlAttribute()
        {
            var item = new AttributedItem { Value = "v1" };
            var serializer = new XmlSerializer(typeof(AttributedItem));
            string xml;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, item);
                xml = writer.ToString();
            }
            Assert.Contains("value=\"v1\"", xml);
        }

        [Fact]
        public void CanIgnoreProperty()
        {
            var item = new WithIgnoredProperty { Included = "yes", Ignored = "ThisValueMustNotAppear" };
            var serializer = new XmlSerializer(typeof(WithIgnoredProperty));
            string xml;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, item);
                xml = writer.ToString();
            }
            Assert.Contains("yes", xml);
            Assert.DoesNotContain("ThisValueMustNotAppear", xml);
        }

        [Fact]
        public void CanSerializeArray()
        {
            var list = new ItemList { Items = new[] { "a", "b", "c" } };
            var serializer = new XmlSerializer(typeof(ItemList));
            string xml;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, list);
                xml = writer.ToString();
            }
            Assert.Contains("<Items>", xml);
            Assert.Contains("<Item>a</Item>", xml);

            ItemList deserialized;
            using (var reader = new StringReader(xml))
            {
                deserialized = (ItemList)serializer.Deserialize(reader);
            }
            Assert.Equal(new[] { "a", "b", "c" }, deserialized.Items);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class AttributedItem
    {
        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class WithIgnoredProperty
    {
        public string Included { get; set; }

        [XmlIgnore]
        public string Ignored { get; set; }
    }

    public class ItemList
    {
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public string[] Items { get; set; }
    }
}
