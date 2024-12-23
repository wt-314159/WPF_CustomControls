﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_CustomControls.Converters
{
    internal class CanvasCenteringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
            {
                return 0;
            }
            if (values[0] is double canvasSize && values[1] is double objectSize)
            {
                var center = (canvasSize - objectSize) / 2;
                if (parameter is not null)
                {
                    if (parameter is double scale)
                    {
                        return center * scale;
                    }
                    else if (parameter is string paramString && double.TryParse(paramString, out var scale2))
                    {
                        return center * scale2;
                    }
                }
                return center;
            }
            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
