using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSide.Common.Spider
{
    public class PageNumSpiderSetting : XmlSetting
    {
        private int fromNum;
        private int toNum;
        private string urlFormat;
        private bool checkExist;
        private string fetchRegex;
        private string savePath;

        public int FromNum
        {
            get
            {
                return fromNum;
            }

            set
            {
                fromNum = value;
            }
        }

        public int ToNum
        {
            get
            {
                return toNum;
            }

            set
            {
                toNum = value;
            }
        }

        public string UrlFormat
        {
            get
            {
                return urlFormat;
            }

            set
            {
                urlFormat = value;
            }
        }

        public bool CheckExist
        {
            get
            {
                return checkExist;
            }

            set
            {
                checkExist = value;
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
