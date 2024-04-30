using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Utils
{
    public static class UnionExtensions
    {
        public static T GetUnionPropertyValue<T>(this object unionObject, string property)
        {
            var value = unionObject.GetUnionPopulatedFieldValue();

            return value.GetPropertyValue<T>(property);
        }

        public static void SetUnionPropertyValue(this object unionObject, string property, object propertyValue)
        {
            var value = unionObject.GetUnionPopulatedFieldValue();

            value.SetPropertyValue(property, propertyValue);
        }

        public static T GetPopulatedFieldValue<T>(this object unionObject)
        {
            return (T)GetUnionPopulatedFieldValue(unionObject);
        }

        public static object GetUnionPopulatedFieldValue(this object unionObject)
        {
            var type = unionObject.GetType();
            var structLayout = type.StructLayoutAttribute;

            if (structLayout == null || structLayout.Value != LayoutKind.Explicit)
            {
                throw new Exception("Invalid union type.  Expects StructLayout.Value = LayoutKind.Explicit");
            }
            else
            {
                var fields = type.GetFields().Cast<FieldInfo>().Where(f => f.GetCustomAttribute<FieldOffsetAttribute>().Value == 0);
                object valueFieldObject = null;

                if (fields.Count() == 0)
                {
                    throw new Exception("Invalid union type.  Expects fields at offset 0");
                }
                else
                {
                    foreach (var field in fields)
                    {
                        var value = field.GetValue(unionObject);

                        if (valueFieldObject != null)
                        {
                            throw new Exception("Invalid union type.  Expects only one field at offset 0 to have value");
                        }
                        else if (value != null)
                        {
                            valueFieldObject = value;
                            break;
                        }
                    }
                }

                return valueFieldObject;
            }
        }
    }
}
