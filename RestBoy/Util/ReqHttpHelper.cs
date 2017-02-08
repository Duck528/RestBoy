using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RestBoy.Util
{
    public class HttpRespVo
    {
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public string RespText { get; set; }
        public int? StatusCode { get; set; }
        public string StatusMsg { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class FileParam
    {
        public FileParam(string fileName, string fileFormat, byte[] binFileData)
        {
            this.FileName = fileName;
            this.FileFormat = fileFormat;
            this.BinFileData = binFileData;
        }
        public FileParam(string fileName, string fileFormat, byte[] binFileData, string contentType) 
            : this(fileName, fileFormat, binFileData)
        {
            this.ContentsType = contentType;
        }
        public byte[] BinFileData { get; set; }
        public string FileName { get; set; }
        public string ContentsType { get; set; }
        public string FileFormat { get; set; }
    }

    class ReqHttpHelper
    {
        public readonly Encoding EncodingType = Encoding.UTF8;
        private static readonly string CRLF = "\r\n";
        private static readonly string ContentType = "multipart/form-data; boundary=";
        private static readonly string UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        public ReqHttpHelper(Encoding encType = null)
        {
            if (encType != null)
                this.EncodingType = encType;
        }

        private bool SetHttpHeaders(HttpWebRequest req, Dictionary<string, string> headers)
        {
            try
            {
                foreach (string key in headers.Keys)
                {
                    switch (key.ToLower())
                    {
                        case "accept":
                            req.Accept = headers[key];
                            break;
                        case "accept-language":
                            req.Headers["accept-language"] = headers[key];
                            break;
                        case "content-type":
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
        private byte[] ToMultipartFormData(Dictionary<string, object> formDatas, string boundary)
        {
            try
            {
                using (var dataStream = new MemoryStream())
                {
                    bool needCLRF = false;
                    foreach (var formData in formDatas)
                    {
                        if (needCLRF == true)
                        {
                            dataStream.Write(this.EncodingType.GetBytes(CRLF), 0, this.EncodingType.GetByteCount(CRLF));
                        }
                        needCLRF = true;

                        var fileParam = formData.Value as FileParam;
                        if (fileParam != null)
                        {
                            string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
                                boundary,
                                formData.Key,
                                fileParam.FileName ?? formData.Key,
                                fileParam.ContentsType ?? "application/octet-stream");

                            // Post 매개변수 헤더를 스트림에 쓴다
                            dataStream.Write(this.EncodingType.GetBytes(header), 0, this.EncodingType.GetByteCount(header));
                            // Post 매개변수 헤더 다음에 바이너리 파일 바이트를 쓴다
                            dataStream.Write(fileParam.BinFileData, 0, fileParam.BinFileData.Length);
                        }
                        else
                        {
                            string postParam = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                                boundary,
                                formData.Key,
                                formData.Value);
                            dataStream.Write(this.EncodingType.GetBytes(postParam), 0, this.EncodingType.GetByteCount(postParam));
                        }
                    }

                    // 요청의 마지막 경계를 작성한다
                    string footer = $"{CRLF}--{boundary}--{CRLF}";
                    dataStream.Write(this.EncodingType.GetBytes(footer), 0, this.EncodingType.GetByteCount(footer));

                    // 스트림을 바이트 배열로 변환한다
                    dataStream.Position = 0;
                    byte[] binData = new byte[dataStream.Length];
                    dataStream.Read(binData, 0, binData.Length);
                    dataStream.Close();
                    return binData;
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
        }
        public async Task<HttpRespVo> Get(string uri, Dictionary<string, string> setHeaders = null)
        {
            var result = new HttpRespVo();

            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(uri);
                webRequest.Method = "GET";
                webRequest.ServicePoint.Expect100Continue = false;
                webRequest.CookieContainer = new CookieContainer();
                webRequest.UserAgent = UserAgent;

                // Set header
                if (setHeaders != null)
                    SetHttpHeaders(webRequest, setHeaders);
                
                using (var response = await webRequest.GetResponseAsync())
                {
                    HttpWebResponse webResponse = (HttpWebResponse)response;
                    // Resp text 
                    Stream respStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(respStream, EncodingType, true);
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
            catch (WebException exp)
            {
                if (exp.Response != null)
                {
                    using (var httpResponse = (HttpWebResponse)exp.Response)
                    {
                        var respStream = httpResponse.GetResponseStream();
                        var reader = new StreamReader(respStream);

                        result.IsSuccess = false;
                        result.RespText = await reader.ReadToEndAsync();
                        result.StatusCode = (int)httpResponse.StatusCode;
                        result.StatusMsg = httpResponse.StatusCode.ToString();
                        return result;
                    }
                }

                // 네트워크에 연결되어 있는지 검사한다
                bool isConnected = NetworkInterface.GetIsNetworkAvailable();
                if (isConnected == false)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "네트워크에 연결되어 있지 않습니다";
                    return result;
                }

                result.IsSuccess = false;
                result.ErrorMsg = exp.Message;
                return result;
            }
            catch (Exception exp)
            {
                result.IsSuccess = false;
                result.ErrorMsg = exp.Message;
                return result;
            }
        }
        public async Task<HttpRespVo> SendMultipart(string uri, string method,
            Dictionary<string, object> formDatas, Dictionary<string, string> setHeaders = null)
        {
            var result = new HttpRespVo();

            try
            {
                var webRequest = WebRequest.Create(uri) as HttpWebRequest;
                if (webRequest == null)
                    throw new InvalidCastException("This request it not a http request");

                // Set method
                webRequest.Method = method;
                // Set cookie container to retrive cookies
                webRequest.CookieContainer = new CookieContainer();
                webRequest.UserAgent = UserAgent;
                // Set headers if not null
                if (setHeaders != null)
                    this.SetHttpHeaders(webRequest, setHeaders);

                // Create boundary randomly and convert formdata
                string boundary = string.Format("----------{0:N}", Guid.NewGuid());
                byte[] binFormData = this.ToMultipartFormData(formDatas, boundary);
                // Set content type
                webRequest.ContentType = ContentType + boundary;
                // Set content length
                webRequest.ContentLength = binFormData.Length;

                // Send form data to uri
                using (Stream reqStream = webRequest.GetRequestStream())
                {
                    reqStream.Write(binFormData, 0, binFormData.Length);
                }

                using (var response = await webRequest.GetResponseAsync())
                {
                    HttpWebResponse webResponse = (HttpWebResponse)response;
                    // Resp text 
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
            catch (WebException exp)
            {
                if (exp.Response != null)
                {
                    using (var httpResponse = (HttpWebResponse)exp.Response)
                    {
                        var respStream = httpResponse.GetResponseStream();
                        var reader = new StreamReader(respStream, UTF8Encoding.UTF8, true);

                        result.IsSuccess = false;
                        result.RespText = await reader.ReadToEndAsync();
                        result.StatusCode = (int)httpResponse.StatusCode;
                        result.StatusMsg = httpResponse.StatusCode.ToString();
                        return result;
                    }
                }

                // 네트워크에 연결되어 있는지 검사한다
                bool isConnected = NetworkInterface.GetIsNetworkAvailable();
                if (isConnected == false)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "네트워크에 연결되어 있지 않습니다";
                    return result;
                }

                result.StatusCode = (int)exp.Status;
                result.StatusMsg = exp.Status.ToString();
                result.IsSuccess = false;
                return result;
            }
            catch (Exception exp)
            {
                result.IsSuccess = false;
                result.ErrorMsg = exp.Message;
                return result;
            }
        }
        
        public async Task<HttpRespVo> SendApplicationJson(string uri, string method, string json,
            Dictionary<string, string> setHeaders)
        {
            var result = new HttpRespVo();

            try
            {
                var webRequest = WebRequest.Create(uri) as HttpWebRequest;
                if (webRequest == null)
                    throw new InvalidCastException("This request it not a http request");

                // Set method
                webRequest.Method = method;
                // Set cookie container to retrive cookies
                webRequest.CookieContainer = new CookieContainer();
                webRequest.UserAgent = UserAgent;
                // Set headers if not null
                if (setHeaders != null)
                    this.SetHttpHeaders(webRequest, setHeaders);
                // Set content type to application/json
                webRequest.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
                
                using (var response = await webRequest.GetResponseAsync())
                {
                    HttpWebResponse webResponse = (HttpWebResponse)response;
                    // Resp text 
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
            catch (WebException exp)
            {
                if (exp.Response != null)
                {
                    using (var httpResponse = (HttpWebResponse)exp.Response)
                    {
                        var respStream = httpResponse.GetResponseStream();
                        var reader = new StreamReader(respStream, UTF8Encoding.UTF8, true);

                        result.IsSuccess = false;
                        result.RespText = await reader.ReadToEndAsync();
                        result.StatusCode = (int)httpResponse.StatusCode;
                        result.StatusMsg = httpResponse.StatusCode.ToString();
                        return result;
                    }
                }

                // 네트워크에 연결되어 있는지 검사한다
                bool isConnected = NetworkInterface.GetIsNetworkAvailable();
                if (isConnected == false)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "네트워크에 연결되어 있지 않습니다";
                    return result;
                }

                result.IsSuccess = false;
                result.ErrorMsg = exp.Message;
                return result;
            }
            catch (Exception exp)
            {
                result.IsSuccess = false;
                result.ErrorMsg = exp.Message;
                return result;
            }
        }
    }
}
