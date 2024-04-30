using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Utils
{
    public class HexNumberTypeConverter : TypeConverter
    {
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
            if (!(value is string))
            {
                ulong ptr;

                if (value is IntPtr intPtr)
                {
                    value = intPtr.ToInt64();
                }

                ptr = Convert.ToUInt64(value);

                return string.Format("0x{0:X8}", ptr);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                ulong ptr;

                if (value is IntPtr intPtr)
                {
                    value = intPtr.ToInt64();
                }

                ptr = Convert.ToUInt64(value);

                return string.Format("0x{0:X8}", ptr);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
