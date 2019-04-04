using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Goofy.Common.Utilities
{
    public class HttpService
    {
        static HttpService()
        {
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
        }

        private static readonly object _obj = new object();
        private static HttpService _instance;
        public static HttpService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new HttpService();
                        }
                    }
                }
                return _instance;
            }
        }
        private HttpService() { }

        public HttpResult Get(string url)
        {
            return Request(new HttpRequestItem() { URL = url });
        }

        public HttpResult Post(string url, byte[] postDatas, string contentType)
        {
            return Request(new HttpRequestItem() { URL = url, Method = "Post", ContentType = contentType, PostDatas = postDatas });
        }

        public HttpResult Request(HttpRequestItem item)
        {
            var request = (HttpWebRequest)WebRequest.Create(item.URL);
            try
            {
                //设置证书
                SetCer(request, item);
                //设置Header
                if (item.Header != null && item.Header.Count > 0)
                {
                    foreach (string key in item.Header.AllKeys)
                    {
                        request.Headers.Add(key, item.Header[key]);
                    }
                }
                //设置代理
                SetProxy(request, item);
                request.Method = item.Method;
                request.Timeout = item.Timeout;
                request.KeepAlive = item.KeepAlive;
                request.ReadWriteTimeout = item.ReadWriteTimeout;
                if (!string.IsNullOrWhiteSpace(item.Host))
                {
                    request.Host = item.Host;
                }
                request.Accept = item.Accept;
                request.ContentType = item.ContentType;
                request.UserAgent = item.UserAgent;
                request.Referer = item.Referer;
                request.AllowAutoRedirect = item.AllowAutoRedirect;
                if (item.MaximumAutomaticRedirections > 0)
                {
                    request.MaximumAutomaticRedirections = item.MaximumAutomaticRedirections;
                }
                //设置安全凭证
                request.Credentials = item.ICredentials;
                //设置Cookie
                SetCookie(request, item);
                //设置Post数据
                SetPostData(request, item);
                //设置最大连接
                if (item.Connectionlimit > 0)
                {
                    request.ServicePoint.ConnectionLimit = item.Connectionlimit;
                }
            }
            catch (Exception ex)
            {
                return new HttpResult() { Html = ex.Message, StatusDescription = $"请求参数错误：{ex.Message}" };
            }

            try
            {
                //请求数据
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return GetResponse(response, item);
                }
            }
            catch (WebException ex)
            {
                using (var response = (HttpWebResponse)ex.Response)
                {
                    return GetResponse(response, item);
                }
            }
            catch (Exception ex)
            {
                return new HttpResult() { Html = ex.Message, StatusDescription = ex.Message };
            }
        }

        /// <summary>
        /// 设置证书
        /// </summary>
        private void SetCer(HttpWebRequest request, HttpRequestItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.CerPath))
            {
                //将证书添加到请求里
                request.ClientCertificates.Add(new X509Certificate(item.CerPath));
            }
            else
            {
                //多个证书
                SetCerList(request, item);
            }
        }
        /// <summary>
        /// 设置多个证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCerList(HttpWebRequest request, HttpRequestItem item)
        {
            if (item.ClentCertificates != null && item.ClentCertificates.Count > 0)
            {
                foreach (X509Certificate c in item.ClentCertificates)
                {
                    request.ClientCertificates.Add(c);
                }
            }
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        private void SetProxy(HttpWebRequest request, HttpRequestItem item)
        {
            if (item.WebProxy != null)
            {
                request.Proxy = item.WebProxy;
            }
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        private void SetCookie(HttpWebRequest request, HttpRequestItem item)
        {
            if (!string.IsNullOrEmpty(item.Cookie))
                request.Headers[HttpRequestHeader.Cookie] = item.Cookie;

            if (item.CookieContainer != null)
                request.CookieContainer = item.CookieContainer;
        }

        /// <summary>
        /// 设置Post数据
        /// </summary>
        private void SetPostData(HttpWebRequest request, HttpRequestItem item)
        {
            if (request.Method.Trim().ToLower().Contains("post"))
            {
                if (item.PostDatas != null)
                {
                    request.ContentLength = item.PostDatas.Length;
                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(item.PostDatas, 0, item.PostDatas.Length);
                    }
                }

            }
        }

        /// <summary>
        /// 获取响应结果
        /// </summary>
        private HttpResult GetResponse(HttpWebResponse response, HttpRequestItem item)
        {
            var result = new HttpResult();
            //获取StatusCode
            result.StatusCode = response.StatusCode;
            //获取StatusDescription
            result.StatusDescription = response.StatusDescription;
            //获取Headers
            result.Header = response.Headers;
            //获取CookieCollection
            if (response.Cookies != null) result.CookieCollection = response.Cookies;
            //获取set-cookie
            if (response.Headers["set-cookie"] != null) result.Cookie = response.Headers["set-cookie"];

            byte[] ResponseByte = GetResponseBytes(response);
            result.ResponseBytes = ResponseByte;

            var encoding = Encoding.UTF8;
            if (item.Encoding != null)
                encoding = item.Encoding;
            result.Html = encoding.GetString(ResponseByte);
            return result;
        }

        private byte[] GetResponseBytes(HttpWebResponse response)
        {
            int bufferSize = 1024 * 10;
            using (MemoryStream _stream = new MemoryStream())
            {
                if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        stream.CopyTo(_stream, bufferSize);
                    }
                }
                else if (response.ContentEncoding != null && response.ContentEncoding.Equals("deflate", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        stream.CopyTo(_stream, bufferSize);
                    }
                }
                else
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        stream.CopyTo(_stream, bufferSize);
                    }
                }
                return _stream.ToArray();
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; //总是接受  
        }
    }


    /// <summary>
    /// Http请求模型
    /// </summary>
    public class HttpRequestItem
    {
        public HttpRequestItem()
        {
            Method = "Get";
            Timeout = 100000;
            ReadWriteTimeout = 300000;
            KeepAlive = true;
            Accept = "text/html, application/xhtml+xml, */*";
            UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.86 Safari/537.36";
            AllowAutoRedirect = true;

            Header = new WebHeaderCollection();
        }

        /// <summary>
        /// 请求URL 必须填写
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 请求方式 默认Get
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 默认请求超时时间 毫秒 默认100000
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 获取或设置向流写入或从流读取时超时 毫秒 默认300000
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// KeepAlive 默认true
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Accept 默认text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept { get; set; }

        /// <summary>
        /// ContentType 
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// UserAgent 默认Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.86 Safari/537.36
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Encoding
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Post请求时要发送的数据
        /// </summary>
        public byte[] PostDatas { get; set; }

        /// <summary>
        /// CookieContainer
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// CookieCollection
        /// </summary>
        public CookieCollection CookieCollection { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// Referer
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath { get; set; }

        /// <summary>
        /// 代理
        /// </summary>
        public WebProxy WebProxy { get; set; }

        /// <summary>
        /// 自动跳转页面，默认true
        /// </summary>
        public bool AllowAutoRedirect { get; set; }

        /// <summary>
        /// 最大连接数 默认50
        /// </summary>
        public int Connectionlimit { get; set; }

        /// <summary>
        /// Header对象
        /// </summary>
        public WebHeaderCollection Header { get; set; }

        /// <summary>
        /// 设置509证书集合
        /// </summary>
        public X509CertificateCollection ClentCertificates { get; set; }

        /// <summary>
        /// Cookie返回类型,默认的是只返回字符串类型
        /// </summary>
        public ResponseCookieType ResponseCookieType { get; set; }

        /// <summary>
        /// 获取或设置请求的身份验证信息。
        /// </summary>
        public ICredentials ICredentials { get; set; }

        /// <summary>
        /// 设置请求将跟随的重定向的最大数目 默认50
        /// </summary>
        public int MaximumAutomaticRedirections { get; set; }

    }

    /// <summary>
    /// Http响应结果
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie { get; set; }
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResponseBytes { get; set; }
        /// <summary>
        /// Header对象
        /// </summary>
        public WebHeaderCollection Header { get; set; }
        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string StatusDescription { get; set; }
        /// <summary>
        /// 返回状态码,默认为OK
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
    }

    /// <summary>
    /// Cookie返回类型
    /// </summary>
    public enum ResponseCookieType
    {
        /// <summary>
        /// 只返回字符串类型的Cookie
        /// </summary>
        String,
        /// <summary>
        /// CookieCollection格式的Cookie集合同时也返回String类型的cookie
        /// </summary>
        CookieCollection
    }
}
