using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Utils
{
    [DebuggerDisplay(" { DebugInfo } "), Serializable()]
    public class CustomAttributeTypedArgument
    {
        [XmlElement]
        public string ArgumentType { get; set; }
        [XmlElement]
        public string Value { get; set; }

        public CustomAttributeTypedArgument()
        {
        }

        public CustomAttributeTypedArgument(System.Reflection.CustomAttributeTypedArgument arg)
        {
            this.ArgumentType = arg.ArgumentType.FullName;
            this.Value = arg.Value.ToString();
        }

        public CustomAttributeTypedArgument(XElement element)
        {
            foreach (var subElement in element.Elements())
            {
                switch (subElement.Name.LocalName)
                {
                    case "ArgumentType":
                        ArgumentType = subElement.Value;
                        break;
                    case "Value":
                        Value = subElement.Value;
                        break;
                    default:
                        Debugger.Break();
                        break;
                }
            }
        }

        public string DebugInfo
        {
            get
            {
                return string.Format("ArgumentType={0}, Value={1}", this.ArgumentType, this.Value);
            }
        }
    }
}
