﻿using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RestBoy.Converter
{
    class JTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var jsonType = (JType)Enum.Parse(typeof(JType), value.ToString());
            switch (jsonType)
            {
                case JType.Object:
                    return new SolidColorBrush(Colors.DarkCyan);

                case JType.Array:
                    return new BrushConverter().ConvertFrom("#c4a000");

                case JType.File:
                    return new SolidColorBrush(Colors.Black);

                case JType.Value:
                    return new BrushConverter().ConvertFrom("#4e9a06");

                default:
                    return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}