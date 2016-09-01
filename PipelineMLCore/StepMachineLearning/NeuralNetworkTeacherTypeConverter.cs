using Accord.Neuro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class NeuralNetworkTeacherTypeConverter : TypeConverter
    {
        // Fields  
        private static Type[] types = new Type[] {
            typeof(SigmoidFunction), typeof(BipolarSigmoidFunction), typeof(ThresholdFunction)
        };

        private StandardValuesCollection values;

        // Methods  
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertTo(context, sourceType));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if ((value == null) || (value.GetType() != typeof(string)))
            {
                return base.ConvertFrom(context, culture, value);
            }
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].ToString().Equals(value))
                {
                    return types[i];
                }
            }
            return typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
            object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return string.Empty;
                }
                value.ToString();
            }
            if ((value != null) && (destinationType == typeof(InstanceDescriptor)))
            {
                object obj2 = value;
                if (value is string)
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (types[i].ToString().Equals(value))
                        {
                            obj2 = types[i];
                        }
                    }
                }
                if ((value is Type) || (value is string))
                {
                    MethodInfo method = typeof(Type).GetMethod("GetType", new Type[] { typeof(string) });
                    if (method != null)
                    {
                        return new InstanceDescriptor(method, new object[] { ((Type)obj2).AssemblyQualifiedName });
                    }
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (values == null)
            {
                object[] objArray;
                if (types != null)
                {
                    objArray = new object[types.Length];
                    Array.Copy(types, objArray, types.Length);
                }
                else
                {
                    objArray = null;
                }
                values = new StandardValuesCollection(objArray);
            }
            return values;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}