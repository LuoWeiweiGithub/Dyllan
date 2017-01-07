using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSide.Common.Spider
{
    public class FileTaskSpiderSetting : XmlSetting
    {
        private string taskFilePath;
        private string saveFilePath;
        private string fetchRegex;

        public string TaskFilePath
        {
            get
            {
                return taskFilePath;
            }

            set
            {
                taskFilePath = value;
            }
        }

        public string SaveFilePath
        {
            get
            {
                return saveFilePath;
            }

            set
            {
                saveFilePath = value;
            }
        }

        public string FetchRegex
        {
            get
            {
                return fetchRegex;
            }

            set
            {
                fetchRegex = value;
            }
        }
    }
}
