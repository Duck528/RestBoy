using RestBoy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RestBoy.Util
{
    public class EnumarableToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable)
            {
                var collection = ((IEnumerable<ParamModel>)value).OrderBy(x => x.Order);
                if (collection.Count() == 0)
                    return string.Empty;

                var builder = new StringBuilder();
                foreach (var v in collection)
                {
                    builder.Append(v.Key).Append("?").Append(v.Value).Append("&");
                }
                builder.Remove(builder.Length - 1, 1);
                return builder.ToString();
            }
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
