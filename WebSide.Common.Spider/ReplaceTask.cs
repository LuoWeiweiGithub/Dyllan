using Dyllan.Common;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace WebSide.Common.Spider
{
    public class ReplaceTask : ITask
    {
        private readonly string filePath;
        private readonly string savePath;
        private readonly List<KeyValuePair> keyValuePairs;

        public ReplaceTask(ReplaceTaskSetting replaceTaskSetting)
        {
            this.filePath = replaceTaskSetting.FilePath;
            this.savePath = replaceTaskSetting.SavePath;
            keyValuePairs = replaceTaskSetting.KeyValuePairs;
        }

        public void Run()
        {
            string content = string.Empty;
            using (StreamReader sr = new StreamReader(filePath))
            {
                content = sr.ReadToEnd();
            }

            string replace = content;
            foreach (KeyValuePair keyValue in keyValuePairs)
            {
                Regex regex = new Regex(keyValue.Key);
                replace = regex.Replace(replace, keyValue.Value);
            }

            Utils.WriteToFile(savePath, replace, true);
        }
    }
}
