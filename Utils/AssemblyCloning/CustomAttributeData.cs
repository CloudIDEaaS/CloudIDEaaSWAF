using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Runtime.CompilerServices;

namespace Utils
{ 
    [DebuggerDisplay(" { DebugInfo } "), Serializable, XmlRoot("CustomAttributeData")]
    public class CustomAttributeData
    {
        [XmlElement]
        public string AttributeType { get; set; }
        [XmlArray("ConstructorArguments")]
        [XmlArrayItem(typeof(CustomAttributeTypedArgument), ElementName = "CustomAttributeTypedArgument")]
        public List<CustomAttributeTypedArgument> ConstructorArguments { get; set; }
        [XmlArray("NamedArguments")]
        [XmlArrayItem(typeof(CustomAttributeNamedArgument), ElementName = "CustomAttributeNamedArgument")]
        public List<CustomAttributeNamedArgument> NamedArguments { get; set; }

        public CustomAttributeData()
        {
        }

        public CustomAttributeData(System.Reflection.CustomAttributeData data)
        {
            var dataDynamic = data as dynamic;

            this.AttributeType = dataDynamic.AttributeType.FullName;
            this.ConstructorArguments = new List<CustomAttributeTypedArgument>();
            this.NamedArguments = new List<CustomAttributeNamedArgument>();

            foreach (var arg in ((IList<System.Reflection.CustomAttributeTypedArgument>)dataDynamic.ConstructorArguments))
            {
                this.ConstructorArguments.Add(new CustomAttributeTypedArgument(arg));
            }

            foreach (var arg in ((IList<System.Reflection.CustomAttributeNamedArgument>)dataDynamic.NamedArguments))
            {
                this.NamedArguments.Add(new CustomAttributeNamedArgument(arg));
            }
        }

        public CustomAttributeData(XElement element)
        {
            foreach (var subElement in element.Elements())
            {
                switch (subElement.Name.LocalName)
                {
                    case "AttributeType":
                        AttributeType = subElement.Value;
                        break;
                    case "ConstructorArguments":
                        ConstructorArguments = subElement.XPathSelectElements("CustomAttributeTypedArgument").Select(e => new CustomAttributeTypedArgument(e)).ToList();
                        break;
                    case "NamedArguments":
                        NamedArguments = subElement.XPathSelectElements("CustomAttributeNamedArgument").Select(e => new CustomAttributeNamedArgument(e)).ToList();
                        break;
                    default:
                        Debugger.Break();
                        break;
                }
            }
        }
 
        public static List<CustomAttributeData> FromList(IList<System.Reflection.CustomAttributeData> list)
        {
            var returnList = new List<CustomAttributeData>();

            foreach (var data in list)
            {
                returnList.Add(new CustomAttributeData(data));
            }

            return returnList;
        }

        public static IList<System.Reflection.CustomAttributeData> ToList(List<CustomAttributeData> customAttributeDataList)
        {
            var returnList = new List<System.Reflection.CustomAttributeData>();
            var customAttributeType = typeof(System.Reflection.CustomAttributeData);
            var customAttributeTypeName = customAttributeType.FullName;
            var customAttributeConstructor = customAttributeType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(Attribute) }, null);
            var ctorArgsField = customAttributeType.GetField("m_typedCtorArgs", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);
            var namedArgsField = customAttributeType.GetField("m_namedArgs", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);

            Debug.Assert(customAttributeConstructor != null);
            DebugUtils.ThrowIf(customAttributeConstructor == null, () => new NullReferenceException(string.Format("Utils.CustomAttributeData.ToList(System.Reflection.CustomAttributeData list) could not get constructor for type '{0}'", customAttributeTypeName)));

            DebugUtils.ThrowIf(ctorArgsField == null, () => new NullReferenceException(string.Format("Utils.CustomAttributeData.ToList(System.Reflection.CustomAttributeData list) could not get field m_typedCtorArgs for type '{0}'", customAttributeTypeName)));
            DebugUtils.ThrowIf(namedArgsField == null, () => new NullReferenceException(string.Format("Utils.CustomAttributeData.ToList(System.Reflection.CustomAttributeData list) could not get field m_namedArgs for type '{0}'", customAttributeTypeName)));

            foreach (var data in customAttributeDataList)
            {
                System.Reflection.CustomAttributeData attributeData;
                Attribute attribute = null;
                ConstructorInfo constructor;
                var attributeType = Type.GetType(data.AttributeType);
                var parms = new List<object>();
                var types = new List<Type>();
                var ctorArgs = new List<System.Reflection.CustomAttributeTypedArgument>();
                var namedArgs = new List<System.Reflection.CustomAttributeNamedArgument>();

                DebugUtils.ThrowIf(attributeType == null, () => new NullReferenceException(string.Format("Utils.CustomAttributeData.ToList(System.Reflection.CustomAttributeData list) could not get type for '{0}'", data.AttributeType)));

                if (data.ConstructorArguments.Count > 0)
                {
                    foreach (var arg in data.ConstructorArguments)
                    {
                        var type = Type.GetType(arg.ArgumentType);

                        DebugUtils.ThrowIf(type == null, () => new NullReferenceException(string.Format("Utils.CustomAttributeData.ToList(System.Reflection.CustomAttributeData list) could not get type for '{0}'", arg.ArgumentType)));

                        types.Add(type);
                    }

                    constructor = attributeType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, types.ToArray(), null);

                    DebugUtils.ThrowIf(constructor == null, () => new NullReferenceException(string.Format("Utils.CustomAttributeData.ToList(System.Reflection.CustomAttributeData list) could not get constructor for '{0}'", attributeType.FullName)));

                    foreach (var arg in data.ConstructorArguments)
                    {
                        var type = Type.GetType(arg.ArgumentType);
                        var value = arg.Value.Convert(type);
                        var ctorArg = new System.Reflection.CustomAttributeTypedArgument(type, value);

                        ctorArgs.Add(ctorArg);
                        parms.Add(value);
                    }

                    attribute = (Attribute)constructor.Invoke(parms.ToArray());
                }
                else
                {
                    constructor = attributeType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, types.ToArray(), null);

                    DebugUtils.ThrowIf(constructor == null, () => new NullReferenceException(string.Format("Utils.CustomAttributeData.ToList(System.Reflection.CustomAttributeData list) could not get constructor for '{0}'", attributeType.FullName)));

                    attribute = (Attribute)constructor.Invoke(parms.ToArray());
                }

                if (data.NamedArguments.Count > 0)
                {
                    types = new List<Type>();

                    foreach (var arg in data.NamedArguments)
                    {
                        var type = Type.GetType(arg.ArgumentType);
                        var value = arg.Value.Convert(type);
                        System.Reflection.CustomAttributeNamedArgument namedArg;

                        if (arg.IsField)
                        {
                            var fieldInfo = attributeType.GetField(arg.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty);

                            DebugUtils.ThrowIf(fieldInfo == null, () => new NullReferenceException(string.Format("Utils.CustomAttributeData.ToList(System.Reflection.CustomAttributeData list) could not get field '{0}' for '{0}'", arg.MemberName, attributeType.FullName)));
                            namedArg = new System.Reflection.CustomAttributeNamedArgument(fieldInfo, value);

                            fieldInfo.SetValue(attribute, value);
                        }
                        else
                        {
                            var propertyInfo = attributeType.GetProperty(arg.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty);

                            DebugUtils.ThrowIf(propertyInfo == null, () => new NullReferenceException(string.Format("Utils.CustomAttributeData.ToList(System.Reflection.CustomAttributeData list) could not get property '{0}' for '{0}'", arg.MemberName, attributeType.FullName)));
                            namedArg = new System.Reflection.CustomAttributeNamedArgument(propertyInfo, value);

                            propertyInfo.GetSetMethod().Invoke(attribute, new object[] { value });
                        }

                        namedArgs.Add(namedArg);
                    }
                }

                attributeData = (System.Reflection.CustomAttributeData)customAttributeConstructor.Invoke(new object[] { attribute });

                ctorArgsField.SetValue(attributeData, ctorArgs);
                namedArgsField.SetValue(attributeData, namedArgs);

                returnList.Add(attributeData);
            }

            return returnList;
        }

        public string DebugInfo
        {
            get
            {
                return string.Format("AttributeType={0}", this.AttributeType);
            }
        }
    }
}
