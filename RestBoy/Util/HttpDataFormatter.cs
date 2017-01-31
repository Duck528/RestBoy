using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBoy.Util
{
    interface IHttpFormatter
    {
        string ConvertTo(List<BodyModel> bodies);
    }
    class JsonFormatter : IHttpFormatter
    {
        public string ConvertTo(List<BodyModel> bodies)
        {
            var builder = new StringBuilder();
            builder.Append("{");
            foreach (var body in bodies)
            {
                try
                {
                    if ("".Equals(body.Key))
                        continue;

                    switch (body.ValueType)
                    {
                        case "File":
                            if ("".Equals(body.FilePath))
                                continue;

                            byte[] fileData = File.ReadAllBytes(body.FilePath);
                            string base64EncodeText = Convert.ToBase64String(fileData);

                            builder.Append("\"").Append(body.Key).Append("\":");
                            builder.Append("\"").Append(base64EncodeText).Append("\",");
                            break;

                        case "Text":
                            if ("".Equals(body.Value))
                                continue;

                            builder.Append("\"").Append(body.Key).Append("\":");
                            builder.Append("\"").Append(body.Value).Append("\",");
                            break;
                    }
                }
                catch (FileNotFoundException exp)
                {
                    Console.WriteLine(exp.ToString());
                }
            }
            builder.Remove(builder.Length, 1);
            builder.Append("}");

            return builder.ToString();
        }
    }
}
