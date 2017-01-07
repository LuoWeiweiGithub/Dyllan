using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WebSide.Common.Spider
{
    public class FileTaskSpider : AbstractRegexSpider
    {
        private readonly string taskFilePath;

        public FileTaskSpider(FileTaskSpiderSetting fileTaskSpiderSetting)
        {
            this.taskFilePath = fileTaskSpiderSetting.TaskFilePath;
            savePath = fileTaskSpiderSetting.SaveFilePath;
            regex = new Regex(fileTaskSpiderSetting.FetchRegex);
        }

        protected override IList<string> GetAvailableTasks()
        {
            IList<string> tasks = new List<string>();
            HashSet<string> urls = new HashSet<string>();
            using (StreamReader sr = new StreamReader(taskFilePath))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (urls.Add(line) && !string.IsNullOrWhiteSpace(line))
                    {
                        tasks.Add(line);
                    }

                    line = sr.ReadLine();
                }
            }
            return tasks;
        }

        protected override void GetLine(StringBuilder result, string line)
        {
            result.AppendLine(line);
        }
    }
}
