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
    class HttpMethodToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string method = System.Convert.ToString(value);
            if (method == null)
                return null;

            switch (method)
            {
                case "GET":
                    return new SolidColorBrush(Colors.Green);

                case "POST":
                    return new SolidColorBrush(Colors.Orange);

                case "DELETE":
                    return new SolidColorBrush(Colors.Black);

                case "PUT":
                    return new SolidColorBrush(Colors.SkyBlue);

                default:
                    return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
