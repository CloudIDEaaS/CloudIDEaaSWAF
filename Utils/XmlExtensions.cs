using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Polenter;
using Polenter.Serialization;
using Polenter.Serialization.Core;
using Polenter.Serialization.Advanced;
using Polenter.Serialization.Advanced.Serializing;
using Polenter.Serialization.Advanced.Deserializing;
using Polenter.Serialization.Serializing;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace Utils
{
    internal class PropertySerializer : PropertyProvider, IPropertySerializer, IPropertyDeserializer
    {
        private Func<Type, PropertyInfo, bool> propertyHandlerCallback;
        private Func<Type, bool> collectionItemHandlerCallback;

        public PropertySerializer(object topLevelObject, Func<Type, PropertyInfo, bool> propertyHandlerCallback, Func<Type, bool> collectionItemHandlerCallback)
        {
            this.propertyHandlerCallback = propertyHandlerCallback;
            this.collectionItemHandlerCallback = collectionItemHandlerCallback;
        }

        public void Close()
        {
        }

        public void Open(Stream stream)
        {
        }

        public void Serialize(Property property)
        {
        }

        public Property Deserialize()
        {
            return null;
        }

        protected override bool IgnoreProperty(Polenter.Serialization.Serializing.TypeInfo info, PropertyInfo property)
        {
            return !propertyHandlerCallback(info.Type, property);
        }

        public override bool IgnoreCollectionItems(Polenter.Serialization.Serializing.TypeInfo info)
        {
            return !collectionItemHandlerCallback(info.Type);
        }
    }

    public static class XmlExtensions
    {
        public static bool IsValidXml(string xmlString)
        {
            Regex tagsWithData = new Regex("<\\w+>[^<]+</\\w+>");

            //Light checking
            if (string.IsNullOrEmpty(xmlString) || tagsWithData.IsMatch(xmlString) == false)
            {
                return false;
            }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int GetElementTextOffset(string content, IXmlLineInfo lineInfo)
        {
            var contentChars = content.ToList();    
            var line = lineInfo.LineNumber;
            var pos = lineInfo.LinePosition;
            var currentLine = 1;
            var currentPos = 1;
            var x = 0;

            foreach (var ch in contentChars)
            {
                if (currentLine == line && currentPos == pos)
                {
                    break;
                }

                if (ch == '\n')
                {
                    currentPos = -1;
                    currentLine++;
                }

                x++;
                currentPos++;
            }

            return x;
        }

        public static string GetContent(this XDocument document)
        {
            var declaration = document.Declaration.ToString();

            return declaration + "\r\n" + document.GetPrivateFieldValue<XElement>("content").ToString();
        }

        public static void Save(this XDocument document, string file, XmlWriterSettings settings)
        {
            using (var stream = File.Create(file))
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    document.Save(writer);
                }
            }
        }

        public static void SetDefaultXmlNamespace(this XElement element, XNamespace xmlns)
        {
            if (element.Name.NamespaceName == string.Empty)
            {
                element.Name = xmlns + element.Name.LocalName;
            }

            foreach (var e in element.Elements())
            {
                e.SetDefaultXmlNamespace(xmlns);
            }
        }

        public static XElement WithDefaultXmlNamespace(this XElement xelem, XNamespace xmlns)
        {
            XName name;

            if (xelem.Name.NamespaceName == string.Empty)
            {
                name = xmlns + xelem.Name.LocalName;
            }
            else
            {
                name = xelem.Name;
            }

            return new XElement(name, from e in xelem.Elements() select e.WithDefaultXmlNamespace(xmlns));
        }

        public static XmlNamespaceManager CreateNamespaceManager(this XDocument document)
        {
            var manager = new XmlNamespaceManager(new NameTable());

            foreach (XAttribute attribute in document.XPathSelectElement("/*").Attributes())
            {
                if (attribute.Name.NamespaceName == "http://www.w3.org/2000/xmlns/")
                {
                    manager.AddNamespace(attribute.Name.LocalName, attribute.Value);
                }
                else if (attribute.Name.LocalName == "xmlns")
                {
                    manager.AddNamespace("xs", attribute.Value);
                }
            }

            return manager;
        }

        public static XmlNamespaceManager CreateNamespaceManager(this XDocument document, Dictionary<string, string> namespaces = null)
        {
            return document.CreateNamespaceManager(null, namespaces);
        }

        public static XmlNamespaceManager CreateNamespaceManager(this XDocument document, string defaultNamespace = null, Dictionary<string, string> namespaces = null)
        {
            var manager = new XmlNamespaceManager(new NameTable());

            if (defaultNamespace != null)
            {
                document.Root.SetDefaultXmlNamespace(defaultNamespace);
            }

            foreach (XAttribute attribute in document.XPathSelectElement("/*").Attributes())
            {
                if (attribute.Name.NamespaceName == "http://www.w3.org/2000/xmlns/")
                {
                    manager.AddNamespace(attribute.Name.LocalName, attribute.Value);
                }
                else if (attribute.Name.LocalName == "xmlns")
                {
                    manager.AddNamespace("xs", attribute.Value);
                }
            }

            if (namespaces != null && namespaces.Count > 0)
            {
                foreach (var pair in namespaces)
                {
                    manager.AddNamespace(pair.Key, pair.Value);
                }
            }

            return manager;
        }

        public static T ToObject<T>(this XElement element)
        {
            var serializer = new SharpSerializer();
            var text = element.ToString();

            using (var stream = text.ToStream())
            {
                var obj = serializer.Deserialize(stream);

                return (T)obj;
            }
        }

        public static string ToXml(this object obj, Func<Type, PropertyInfo, bool> propertyHandlerCallback, Func<Type, bool> collectionItemHandlerCallback)
        {
            var settings = new SharpSerializerXmlSettings
            {
                AdvancedSettings = new AdvancedSharpSerializerXmlSettings
                {
                    TypeNameConverter = new TypeNameConverter()
                },

                IncludeAssemblyVersionInTypeName = false,
                IncludeCultureInTypeName = false,
                IncludePublicKeyTokenInTypeName = false,
            };

            var propertySerializer = new PropertySerializer(obj, propertyHandlerCallback, collectionItemHandlerCallback);
            var serializer = new SharpSerializer() { PropertyProvider = propertySerializer, RootName=obj.GetType().Name };

            using (var stream = new MemoryStream())
            {
                var text = string.Empty;
                string xml;

                serializer.Serialize(obj, stream);

                stream.Seek(0, SeekOrigin.Begin);

                xml = stream.ToText();

                return xml;
            }
        }

        public static string ToXml(this object obj)
        {
            var settings = new SharpSerializerXmlSettings
            {
                AdvancedSettings = new AdvancedSharpSerializerXmlSettings
                {
                    TypeNameConverter = new TypeNameConverter()
                },

                IncludeAssemblyVersionInTypeName = false,
                IncludeCultureInTypeName = false,
                IncludePublicKeyTokenInTypeName = false,
            };

            var serializer = new SharpSerializer() { RootName = obj.GetType().Name };

            using (var stream = new MemoryStream())
            {
                var text = string.Empty;
                string xml;

                serializer.Serialize(obj, stream);

                stream.Seek(0, SeekOrigin.Begin);

                xml = stream.ToText();

                return xml;
            }
        }

        public static T GetAttributeValue<T>(this XElement element, XName attributeName)
        {
            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), element.Attributes().Single(a => a.Name.LocalName == attributeName.LocalName).Value);
            }
            else
            {
                var attribute = element.Attribute(attributeName);

                if (attribute != null)
                {
                    if (typeof(T) == typeof(byte))
                    {
                        return (T)(object) byte.Parse(attribute.Value);
                    }
                    else if (typeof(T) == typeof(IntPtr))
                    {
                        return (T)(object)(IntPtr)int.Parse(attribute.Value);
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        return (T)(object)int.Parse(attribute.Value);
                    }
                    else if (typeof(T) == typeof(long))
                    {
                        return (T)(object)long.Parse(attribute.Value);
                    }
                    else if (typeof(T) == typeof(uint))
                    {
                        return (T)(object)uint.Parse(attribute.Value);
                    }
                    else if (typeof(T) == typeof(ulong))
                    {
                        return (T)(object)ulong.Parse(attribute.Value);
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        return (T)(object)float.Parse(attribute.Value);
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        return (T)(object)double.Parse(attribute.Value);
                    }
                    else if (typeof(T) == typeof(bool))
                    {
                        return (T)(object)bool.Parse(attribute.Value);
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        return (T)(object)attribute.Value;
                    }
                    else if (typeof(T) == typeof(DateTime))
                    {
                        return (T)(object)DateTime.Parse(attribute.Value);
                    }

                    Debugger.Break();
                    return default(T);
                }
                else
                {
                    return default(T);
                }
            }
        }

        public static string RemoveNamespace(this string text)
        {
            var index = text.IndexOf(':');

            if (index != -1)
            {
                return text.RemoveStart(index + 1);
            }
            else
            {
                return text;
            }
        }
    }
}
