using Dyllan.Common;
using NLog;
using System;
using System.Threading.Tasks;

namespace WebSide.Common.Spider
{
    public class CustomTask
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly TaskSetting taskSetting;
        public CustomTask(string file)
        {
            DXmlDeserializer<TaskSetting> deserializer = new DXmlDeserializer<TaskSetting>(file);
            taskSetting = deserializer.Deserialize();
        }

        public void ExecuteInSequence()
        {
            try
            {
                int i = 0;
                foreach (XmlSetting xmlSetting in taskSetting.Tasks)
                {
                    i++;
                    ExecuteTask(xmlSetting);
                    logger.Info("Finished task[{0}].", i);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void ExecuteInParallel()
        {
            try
            {
                Parallel.ForEach(taskSetting.Tasks, ExecuteTask);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void ExecuteTask(XmlSetting xmlSetting)
        {
            logger.Info("Begin task:{0}", xmlSetting.TaskName);
            ITask task = TaskFactory.CreateTask(xmlSetting);
            task.Run();
            logger.Info("End task:{0}", xmlSetting.TaskName);
        }
    }
}
