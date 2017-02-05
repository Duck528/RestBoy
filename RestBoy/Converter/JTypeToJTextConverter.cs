using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RestBoy.Converter
{
    class JTypeToJTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var jsonType = (JType)Enum.Parse(typeof(JType), values[0].ToString());
            string key = System.Convert.ToString(values[1]);
            string value = System.Convert.ToString(values[2]);
            bool isReadonly = (bool)values[3];

            switch (jsonType)
            {
                case JType.Object:
                    if (isReadonly == true)
                        return "{ }:";
                    else
                        return "\"" + key + "\"" + " : { }";

                case JType.Array:
                    return "\"" + key + "\"" + " : [ ]";

                case JType.File:
                    if ("선택된 파일이 없습니다".Equals(value))
                        return "\"" + key + "\"" + " : \"" + Path.GetFileName(value) + "\"";
                    else
                        return "\"" + key + "\"" + " : \"(base64)" + Path.GetFileName(value) + "\"";

                case JType.String:
                    if ("".Equals(key.Trim()))
                        return "\"" + value + "\"";
                    else
                        return "\"" + key + "\"" + " : " + value + "\"";

                case JType.Number:
                    if ("".Equals(key.Trim()))
                        return value;
                    else
                        return "\"" + key + "\"" + " : " + value;

                default:
                    return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
