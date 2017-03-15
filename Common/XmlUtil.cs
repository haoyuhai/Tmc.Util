using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Tmc.Util
{
    public static class XmlUtil
    {
        public static string SerializeToXml(object obj)
        {
            if (obj == null) return string.Empty;

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(sw, obj);

                return sw.ToString();
            }
        }

        public static object DeserializeFromXml(string xml, Type type)
        {
            if (string.IsNullOrWhiteSpace(xml)) return null;

            using (System.IO.StringReader sr = new System.IO.StringReader(xml))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
                return xmlSerializer.Deserialize(sr);
            }
        }

        public static T DeserializeFromXml<T>(string xml) where T : class
        {
            object obj = DeserializeFromXml(xml, typeof(T));
            return (T)obj;
        }

        /// <summary>
        /// 得到节点内的文本值
        /// </summary>
        public static string SelectSingleNodeValue(XmlNode xmlNode, string xpath)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            XmlNode node = xmlNode.SelectSingleNode(xpath);
            return node == null ? string.Empty : node.InnerXml.Trim();
        }

        public static XElement ElementIgnoreCase(this XContainer container, XName name)
        {
            foreach (XElement element in container.Elements())
            {
                if (element.Name.NamespaceName == name.NamespaceName &&
                String.Equals(element.Name.LocalName, name.LocalName, StringComparison.OrdinalIgnoreCase))
                {
                    return element;
                }
            }
            return null;
        }

        public static IEnumerable<XElement> ElementsIgnoreCase(this XContainer container, XName name)
        {
            foreach (XElement element in container.Elements())
            {
                if (element.Name.NamespaceName == name.NamespaceName &&
                String.Equals(element.Name.LocalName, name.LocalName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<XAttribute> AttributesIgnoreCase(this XElement element, XName name)
        {
            foreach (XAttribute attr in element.Attributes())
            {
                if (attr.Name.NamespaceName == name.NamespaceName &&
                String.Equals(attr.Name.LocalName, name.LocalName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return attr;
                }
            }
        }
    } 
}
