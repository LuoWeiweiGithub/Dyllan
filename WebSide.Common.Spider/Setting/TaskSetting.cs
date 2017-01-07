using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSide.Common.Spider
{
    public class TaskSetting : XmlSetting
    {
        private List<XmlSetting> tasks;

        public List<XmlSetting> Tasks
        {
            get
            {
                return tasks;
            }

            set
            {
                tasks = value;
            }
        }
    }
}
