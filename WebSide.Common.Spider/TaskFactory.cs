using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSide.Common.Spider
{
    public static class TaskFactory
    {
        public static ITask CreateTask(XmlSetting xmlSetting)
        {
            ITask task = null;
            if (xmlSetting is PageNumSpiderSetting)
            {
                task = new PageNumSpider((PageNumSpiderSetting)xmlSetting);
            }
            else if (xmlSetting is FileTaskSpiderSetting)
            {
                task = new FileTaskSpider((FileTaskSpiderSetting)xmlSetting);
            }
            else if (xmlSetting is ReplaceTaskSetting)
            {
                task = new ReplaceTask((ReplaceTaskSetting)xmlSetting);
            }
            else
            {
                // nothing to do.
            }
            return task;
        }
    }
}
