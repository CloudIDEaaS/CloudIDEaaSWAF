using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Utils;

namespace Utils
{
    public class NumberTimeConverter : TypeConverter
    {
        private object originalValue;

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return originalValue;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            var number = (uint)value;
            var timeStamp = DateTime.Parse("12/31/1969 4:00 PM");

            originalValue = value;
            timeStamp = timeStamp.AddSeconds(number).ToLocalTime();

            return timeStamp.ToDateTimeText(); 
        }
    }
}
