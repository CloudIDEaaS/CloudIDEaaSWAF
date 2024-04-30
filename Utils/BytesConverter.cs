using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Utils;

namespace Utils
{
    public class BytesConverter : TypeConverter
    {
        private object originalValue;

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return originalValue;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            var bytes = BitConverter.GetBytes((uint)value);

            originalValue = value;

            return bytes.GetHexString() + "  " + bytes.GetDataString();
        }
    }
}
