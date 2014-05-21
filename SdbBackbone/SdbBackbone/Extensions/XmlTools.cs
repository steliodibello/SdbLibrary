using System.IO;
using System.Xml.Serialization;

namespace SdbBackbone.Extensions
{
    public static class XmlTools
    {
        public static string ToXmlString<T>(this T input)
        {
            using (var writer = new StringWriter())
            {
                input.ToXml(writer);
                return writer.ToString();
            }
        }

        public static void ToXml<T>(this T objectToSerialize, Stream stream)
        {
            new XmlSerializer(typeof(T)).Serialize(stream, objectToSerialize);
        }

        public static void ToXml<T>(this T objectToSerialize, StringWriter writer)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("", "http://www.w3.org/2001/XMLSchema");

            new XmlSerializer(typeof(T)).Serialize(writer, objectToSerialize, ns);
        }
    }
}
