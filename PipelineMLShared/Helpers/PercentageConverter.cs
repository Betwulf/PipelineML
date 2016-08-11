using System;
using System.ComponentModel;

namespace PipelineMLCore
{
    public class PercentageConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(double))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is double)
            {
                double lPercentage = ((double)value * 100);
                return lPercentage.ToString() + "%";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string lPercentageString = (string)value;
                    double lPercentage = double.Parse(lPercentageString.Replace("%", "")) / 100;
                    return lPercentage;
                }
                catch
                {
                    throw new ArgumentException("Can not convert '" + (string)value + "' to type Double");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
