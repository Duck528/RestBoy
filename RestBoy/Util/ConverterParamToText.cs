using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RestBoy.Util
{
    class ConverterParamToText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ParamModel)
            {
                var builder = new StringBuilder();
                var paramModel = (ParamModel)value;
                builder.Append(paramModel.Key).Append("=").Append(paramModel.Value);
                return builder.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
