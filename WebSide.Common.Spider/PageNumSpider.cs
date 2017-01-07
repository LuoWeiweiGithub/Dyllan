using Dyllan.Common.Web;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WebSide.Common.Spider
{
    public class PageNumSpider : AbstractRegexSpider
    {
        private int fromNum;
        private int toNum;
        private string urlFormat;
        private bool checkExist;
        private Dictionary<int, HttpHelper> httpHelpers = new Dictionary<int, HttpHelper>();
        private HashSet<string> existLines = new HashSet<string>();

        public PageNumSpider(PageNumSpiderSetting pageNumSpiderSetting)
        {
            this.fromNum = pageNumSpiderSetting.FromNum;
            this.toNum = pageNumSpiderSetting.ToNum;
            this.urlFormat = pageNumSpiderSetting.UrlFormat;
            this.regex = new Regex(pageNumSpiderSetting.FetchRegex);
            this.savePath = pageNumSpiderSetting.SavePath;
            this.checkExist = pageNumSpiderSetting.CheckExist;
            if (checkExist)
            {
                InitResult();
            }
        }

        private void InitResult()
        {
            if (!File.Exists(savePath))
                return;
            using (StreamReader sr = new StreamReader(savePath))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    existLines.Add(line);
                    line = sr.ReadLine();
                }
            }
        }

        protected override void GetLine(StringBuilder result, string line)
        {
            if (!checkExist || !existLines.Contains(line))
            {
                result.AppendLine(line);
            }
        }

        protected override IList<string> GetAvailableTasks()
        {
            IList<string> result = new List<string>();
            for (int i = fromNum; i <= toNum; i++)
            {
                result.Add(string.Format(urlFormat, i));
            }
            return result;
        }

    }
}
