using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestBoy.Util
{
    static class ReqHttpHelper
    {
        private static string CreateUriParam(string uri, List<ParamModel> uriParams)
        {
            var uriBuilder = new StringBuilder(uri);
            uriBuilder.Append("?");
            foreach (var param in uriParams)
            {
                string key = param.Key.Trim();
                if ("".Equals(key) == true)
                    continue;
                else
                    uriBuilder.Append(key);

                string val = param.Value.Trim();
                if ("".Equals(val) == false)
                    uriBuilder.Append("=").Append(val);
                uriBuilder.Append("&");
            }
            uriBuilder.Remove(uriBuilder.Length - 1, 1);
            return uriBuilder.ToString();
        }

        private static HttpWebRequest CompleteHeaders(
            HttpWebRequest sendReq, List<HeaderModel> headers)
        {
            foreach (var header in headers)
            {
                try
                {
                    sendReq.Headers[header.Key] = header.Value;
                }
                catch (KeyNotFoundException exp)
                {
                    Console.WriteLine(exp.ToString());
                }
            }

            return sendReq;
        }

        /*
        public static HttpWebResponse SendPostRequest(
            string uri, string method,
            List<ParamModel> uriParams, List<HeaderModel> uriHeaders, 
            string dataType, List<BodyModel> uriBodies)
        {
            string uriWithParam = CreateUriParam(uri, uriParams);
            var httpReq = (HttpWebRequest)WebRequest.Create(uriWithParam);
            httpReq.Method = "POST";

            switch (dataType)
            {
            }
        }


        public static HttpWebResponse SendRequest(
            string uri, string method, 
            List<ParamModel> uriParams, List<HeaderModel> uriHeaders, List<BodyModel> uriBodies)
        {
            string uriParam = CreateUriParam(uri, uriParams);
            var httpReq = (HttpWebRequest)WebRequest.Create(uriParam);
            httpReq.Method = method;
        }
        */
    }
}
