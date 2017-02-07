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
            bool hasKey = (bool)values[3];
            string fileHeader = System.Convert.ToString(values[4]) ?? "base64";
            fileHeader += ",";

            switch (jsonType)
            {
                case JType.Object:
                    if (hasKey == true)
                        return "\"" + key + "\"" + " : { }";
                    else
                        return "{ }:";

                case JType.Array:
                    if (hasKey == true)
                        return "\"" + key + "\"" + " : [ ]";
                    else
                        return "[ ]:";

                case JType.File:
                    if ("선택된 파일이 없습니다".Equals(value))
                        return "\"" + key + "\"" + " : \"" + Path.GetFileName(value) + "\"";
                    else
                        return "\"" + key + "\"" + $" : \"{fileHeader}(Encoded)" + Path.GetFileName(value) + "\"";

                case JType.String:
                    if (hasKey == true)
                        return "\"" + key + "\"" + " : " + value + "\"";
                    else
                        return "\"" + value + "\"";

                case JType.Number:
                    if (hasKey == true)
                        return "\"" + key + "\"" + " : " + value;
                    else
                        return value;

                case JType.Boolean:
                    if (hasKey == true)
                        return "\"" + key + "\"" + " : " + value;
                    else
                        return value;

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
