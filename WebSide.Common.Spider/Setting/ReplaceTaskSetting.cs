using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebSide.Common.Spider
{
    public class ReplaceTaskSetting : XmlSetting
    {
        private string filePath;
        private string savePath;
        private List<KeyValuePair> keyValuePairs = new List<KeyValuePair>();

        public List<KeyValuePair> KeyValuePairs
        {
            get
            {
                return keyValuePairs;
            }
            set
            {
                keyValuePairs = value;
            }
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }

        public string SavePath
        {
            get
            {
                return savePath;
            }
            set
            {
                savePath = value;
            }
        }
    }
}
