using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_CustomControls.Converters
{
    public class AngleConverter : IValueConverter
    {
        public double Scale { get; set; } = 1;
        public double Offset { get; set; } = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return Offset + (d * Scale);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return (d - Offset) / Scale;
            }
            return value;
        }
    }
}
