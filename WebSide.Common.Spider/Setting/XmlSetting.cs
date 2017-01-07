using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebSide.Common.Spider
{
    public abstract class XmlSetting
    {
        private string taskName;

        public XmlSetting()
        {
        }

        public string TaskName
        {
            get
            {
                return taskName;
            }

            set
            {
                taskName = value;
            }
        }
    }
}
