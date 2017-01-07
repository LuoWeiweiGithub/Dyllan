using Dyllan.Common.Web;
using Dyllan.Common.Web.Spide;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebSide.Common.Spider
{
    public abstract class AbstractRegexSpider : SpiderDirector<string>, ITask
    {
        private string urlFormat;
        private int _interval = 1000;
        protected Regex regex;
        protected string savePath;
        private Dictionary<int, HttpHelper> httpHelpers = new Dictionary<int, HttpHelper>();
        protected abstract void GetLine(StringBuilder result, string line);

        protected override string DoingWork(string task)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                _log.Debug("Analyzing {0}", task);
                MatchCollection matchs = regex.Matches(GetHtml(task));
                foreach (Match match in matchs)
                {
                    string line = match.Value;
                    GetLine(result, line);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            return result.ToString();
        }

        protected override void DoneWork(IList<string> tasks)
        {
            lock (this)
            {
                using (StreamWriter sw = new StreamWriter(savePath, true, Encoding.UTF8))
                {
                    foreach (string task in tasks)
                    {
                        sw.WriteLine(task);
                    }
                }
            }
        }

        private string GetHtml(string url)
        {
            string content = string.Empty;
            try
            {
                HttpHelper httpHelper = GetHttpHelper();
                var response = httpHelper.GetResponse(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    content = httpHelper.GetString(response);
                }
                else
                {
                    response.Close();
                    response.Dispose();
                }
            }
            catch (WebException webEx)
            {
                _log.Error("Get url error: {0}{1}{2}", url, Environment.NewLine, webEx);
            }
            return content;
        }

        private HttpHelper GetHttpHelper()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            lock (httpHelpers)
            {
                if (!httpHelpers.ContainsKey(threadId))
                {
                    HttpHelper httpHelper = new HttpHelper(_interval);
                    httpHelpers.Add(threadId, httpHelper);
                }
            }

            HttpHelper helper = httpHelpers[threadId];
            return helper;
        }
    }
}
