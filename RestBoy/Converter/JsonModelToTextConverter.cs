using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RestBoy.Converter
{
    class JsonModelToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var models = value as ObservableCollection<JsonModel>;
            if (models == null)
                return "";

            var builder = new StringBuilder();
            foreach (var model in models[0].Childs)
            {
                builder.Append(model.ToDisplayJson());
            }

            return builder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
