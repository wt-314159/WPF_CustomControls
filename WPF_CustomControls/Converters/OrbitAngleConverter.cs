using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_CustomControls.Converters
{
    internal class OrbitAngleConverter : IMultiValueConverter
    {
        public double OddIndexOffset { get; set; }
        public double Scale { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
            {
                return double.NaN;
            }
            if (values[0] is double angle && values[1] is int index)
            {
                var offset = index % 2 == 0 ? 0 : 180;
                return (angle * Scale) + offset;
            }
            return double.NaN;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
