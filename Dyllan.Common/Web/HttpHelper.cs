using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dyllan.Common.Web
{
    public class HttpHelper
    {
        private readonly string _contentType = "application/x-www-form-urlencoded";
        public string ContentType
        {
            get
            {
                return _contentType;
            }
        }

        private Encoding _encoding = Encoding.UTF8;

        //private string _userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
        private string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/0.2.149.27 Safari/525.13";
        public string UserAgent
        {
            get
            {
                return _userAgent;
            }
            set
            {
                _userAgent = value;
            }
        }

        CookieContainer _cookieCon = new CookieContainer();
        public CookieContainer Cookie
        {
            get
            {
                return _cookieCon;
            }
            set
            {
                _cookieCon = value;
            }
        }

        private int _timeOut = 30 * 1000;
        public int TimeOut
        {
            get
            {
                return _timeOut;
            }
            set
            {
                _timeOut = value;
            }
        }

        public string Accept
        {
            get;
            set;
        }

        public string Referer
        {
            get;set;
        }

        public HttpHelper()
        {
            ServicePointManager.DefaultConnectionLimit = 100;
        }

        public HttpHelper(int connectionLimit)
        {
            ServicePointManager.DefaultConnectionLimit = connectionLimit;
        }

        public string GetURL(string url)
        {
            HttpWebResponse res = GetResponse(url);
            string strResult = GetString(res);
            return strResult;
        }

        public HttpWebResponse GetResponse(string url)
        {
            HttpWebResponse res = null;
            var req = HttpWebRequest.CreateHttp(url);
            req.Method = "GET";
            req.ContentType = _contentType;
            if (!string.IsNullOrEmpty(Accept))
                req.Accept = Accept;
            if (!string.IsNullOrEmpty(Referer))
                req.Referer = Referer;
            req.UserAgent = _userAgent;
            req.Timeout = _timeOut;
            req.CookieContainer = Cookie;
            res = (HttpWebResponse)req.GetResponse();
            return res;
        }

        public HttpWebResponse GetResponseHeader(string url)
        {
            HttpWebResponse res = null;
            var req = HttpWebRequest.CreateHttp(url);
            req.Method = "HEAD";
            req.ContentType = _contentType;
            if (!string.IsNullOrEmpty(Accept))
                req.Accept = Accept;
            if (!string.IsNullOrEmpty(Referer))
                req.Referer = Referer;
            req.UserAgent = _userAgent;
            req.Timeout = _timeOut;
            req.CookieContainer = Cookie;
            res = (HttpWebResponse)req.GetResponse();
            return res;
        }

        public string GetString(HttpWebResponse response)
        {
            StringBuilder strResult = new StringBuilder();
            try
            {
                Stream receiveStream = response.GetResponseStream();
                string encodeheader = response.ContentType;
                Encoding encode = _encoding;
                var sr = new StreamReader(receiveStream, encode);
                var read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    var str = new String(read, 0, count);
                    strResult.Append(str);
                    count = sr.Read(read, 0, 256);
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }
            return strResult.ToString();
        }

        public void WriteToFile(string filePath, string url)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            HttpWebResponse response = GetResponse(url);
            try
            {
                Stream stream = response.GetResponseStream();
                List<byte[]> content = new List<byte[]>();
                long length = response.ContentLength;
                
                while (length > 8000)
                {
                    byte[] bytes = new byte[8000];
                    ReadBytes(bytes, stream);
                    content.Add(bytes);
                    length -= 8000;
                }

                if (length > 0)
                {
                    byte[] bytes = new byte[length];
                    ReadBytes(bytes, stream);
                    content.Add(bytes);
                }

                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    BinaryWriter bw = new BinaryWriter(sw.BaseStream);
                    foreach (var b in content)
                        bw.Write(b);
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }
        }

        private void ReadBytes(byte[] container, Stream stream)
        {
            int length = container.Length;
            int read = 0;
            int count = stream.Read(container, 0, length);
            while (count > 0)
            {
                read += count;
                count = stream.Read(container, read, length - read);
            }
        }
    }
}
