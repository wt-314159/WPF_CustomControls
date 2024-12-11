using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static WPF_CustomControls.Controls.BusyIndicator;

namespace WPF_CustomControls.Converters
{
    internal class OrbitAngleConverter : IValueConverter
    {
        public double OddIndexOffset { get; set; }
        public double Scale { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AngleAndPhase angleAndPhase)
            {
                var offset = angleAndPhase.Index % 2 == 0 ? 0 : 180;
                return (angleAndPhase.StartAngle * Scale) + offset;
            }
            return double.NaN;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
