using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_CustomControls.Converters
{
    internal class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 0)
            {
                double result = 1;
                foreach (var value in values)
                {
                    if (TryParse(value, out double d))
                    {
                        result *= d;
                    }
                    else
                    {
                        // Could just skip any values that can't be converted to doubles,
                        // but this is more likely to lead to difficult to track down bugs,
                        // so return NaN so we know there is something wrong in the binding
                        return double.NaN;
                    }
                }
                if (parameter is not null && TryParse(parameter, out double param))
                {
                    result *= param;
                }
                return result;
            }
            return double.NaN;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static T Multiply<T>(T[] values) where T : INumber<T>
        {
            T first = values.First();
            for (int i = 1; i < values.Length; i++)
            {
                first *= values[i];
            }
            return first;
        }

        public static bool TryParse(object value, out double d)
        {
            if (value is double @double)
            {
                d = @double;
                return true;
            }
            else if (value is string s)
            {
                return double.TryParse(s, out d);
            }
            else if (value is int i)
            {
                d = (double)i;
                return true;
            }
            d = double.NaN;
            return false;
        }
    }
}
