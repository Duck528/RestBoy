using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestBoy.Util
{
    public class HttpRespVo
    {
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public string RespText { get; set; }
        public int StatusCode { get; set; }
        public string StatusMsg { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
    }

    static class ReqHttpHelper
    {
        private static bool SetHttpHeaders(HttpWebRequest req, Dictionary<string, string> headers)
        {
            try
            {
                foreach (string key in headers.Keys)
                {
                    switch (key)
                    {
                        case "Accept":
                            req.Accept = headers[key];
                            break;
                        case "Content-Type":
                            req.ContentType = headers[key];
                            break;
                    }
                }

                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
        public async static Task<HttpRespVo> Get(string uri, Dictionary<string, string> setHeaders=null)
        {
            try
            {
                Uri targetUri = new Uri(uri);
                var webRequest = (HttpWebRequest)WebRequest.Create(targetUri);
                webRequest.Method = "GET";
                webRequest.ServicePoint.Expect100Continue = false;
                webRequest.CookieContainer = new CookieContainer();

                // 헤더 설정
                if (setHeaders != null)
                {
                    SetHttpHeaders(webRequest, setHeaders);
                }

                var result = new HttpRespVo();
                using (var response = await webRequest.GetResponseAsync())
                {
                    HttpWebResponse webResponse = (HttpWebResponse)response;
                    // RespText 
                    Stream respStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(respStream, UTF8Encoding.UTF8, true);
                    string respText = reader.ReadToEnd();
                    result.RespText = respText;

                    // Headers
                    var headers = new Dictionary<string, string>();
                    foreach (string headerName in webResponse.Headers)
                    {
                        headers.Add(headerName, webResponse.Headers.Get(headerName));
                    }
                    result.Headers = headers;

                    // Cookies
                    var cookies = new Dictionary<string, string>();
                    foreach (Cookie cookie in webResponse.Cookies)
                    {
                        cookies.Add(cookie.Name, cookie.Value);
                    }
                    result.Cookies = cookies;

                    // StatucCode and StatucMsg
                    result.StatusCode = (int)webResponse.StatusCode;
                    result.StatusMsg = webResponse.StatusCode.ToString();
                }
                result.IsSuccess = true;
                return result;
            }
            catch (IOException exp)
            {
                return new HttpRespVo()
                {
                    IsSuccess = false,
                    ErrorMsg = exp.Message
                };
            }
            catch (InvalidCastException exp)
            {
                return new HttpRespVo()
                {
                    IsSuccess = false,
                    ErrorMsg = exp.Message
                };
            }
            catch (Exception exp)
            {
                return new HttpRespVo()
                {
                    IsSuccess = false,
                    ErrorMsg = exp.Message,
                };
            }
        }
        public async static Task<HttpRespVo> PostMultipart(string uri, string urlParams,
            Dictionary<string, string> setHeaders = null, Dictionary<string, string> formData=null)
        {
            return null;
        }
    }
}
