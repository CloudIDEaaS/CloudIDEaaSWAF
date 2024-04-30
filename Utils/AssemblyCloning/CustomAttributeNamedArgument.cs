using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Utils
{
    [DebuggerDisplay(" { DebugInfo } "), Serializable()]
    public class CustomAttributeNamedArgument
    {
        [XmlElement]
        public string ArgumentType { get; set; }
        [XmlElement]
        public string MemberName { get; set; }
        [XmlElement]
        public string Value { get; set; }
        [XmlElement]
        public bool IsField { get; set; }

        public CustomAttributeNamedArgument()
        {
        }

        public CustomAttributeNamedArgument(System.Reflection.CustomAttributeNamedArgument arg)
        {
            if (arg.MemberInfo is FieldInfo)
            {
                this.ArgumentType = ((FieldInfo) arg.MemberInfo).FieldType.FullName;
                this.IsField = true;
            }
            else
            {
                this.ArgumentType = ((PropertyInfo) arg.MemberInfo).PropertyType.FullName;
            }

            this.MemberName = arg.MemberInfo.Name;
            this.Value = arg.TypedValue.Value.ToString();
        }

        public CustomAttributeNamedArgument(XElement element)
        {
            foreach (var subElement in element.Elements())
            {
                switch (subElement.Name.LocalName)
                {
                    case "ArgumentType":
                        ArgumentType = subElement.Value;
                        break;
                    case "MemberName":
                        MemberName = subElement.Value;
                        break;
                    case "Value":
                        Value = subElement.Value;
                        break;
                    case "IsField":
                        IsField = bool.Parse(subElement.Value);
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
                return string.Format("ArgumentType={0}, MemberName={1}, Value={2}, IsField={3}", this.ArgumentType, this.MemberName, this.Value, this.IsField);
            }
        }
    }
}
