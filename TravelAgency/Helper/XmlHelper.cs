using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TravelAgency.Helper
{
    public class XmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName)
            where T : class
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);
            object? deserializedObjects = xmlSerializer.Deserialize(stringReader);
            if (deserializedObjects == null ||
                deserializedObjects is not T deserializedObjectTypes)
            {
                throw new InvalidOperationException();
            }

            return deserializedObjectTypes;
        }

        public string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            // Configure XML writer settings
            var xmlWriterSettings = new XmlWriterSettings
            {
                Encoding = Encoding.Unicode, // Ensure UTF-16 encoding
                Indent = true,
                OmitXmlDeclaration = false // Ensure XML declaration is included
            };

            // Create XML writer and serialize the object
            using (var stringWriter = new StringWriter(sb))
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", ""); // Ensure no namespace is included

                    xmlSerializer.Serialize(xmlWriter, obj, namespaces);
                }
            }

            return sb.ToString().TrimEnd();
        }





    }
}
