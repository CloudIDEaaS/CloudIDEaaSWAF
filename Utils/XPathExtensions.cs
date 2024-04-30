using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Utils.XPath
{
    public static class XPathExtensions
    {
        public static string MergeAttributes(this string xpathExpression, string newAttributesPart)
        {
            return xpathExpression.RegexReplace("]$", " and " + newAttributesPart + "]");
        }

        public static XElement XPathSelectElement(this FileInfo file, string path)
        {
            using (var reader = new StreamReader(file.FullName))
            {
                var document = XDocument.Load(reader);

                return document.XPathSelectElement(path);
            }
        }

        public static IEnumerable<XElement> XPathSelectElements(this FileInfo file, string path)
        {
            using (var reader = new StreamReader(file.FullName))
            {
                var document = XDocument.Load(reader);

                return document.XPathSelectElements(path);
            }
        }
    }
}
