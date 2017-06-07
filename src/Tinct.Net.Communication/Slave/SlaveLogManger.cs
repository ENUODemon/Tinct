using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Common.Log;
using Tinct.Net.Message.Task;

namespace Tinct.Net.Communication.Slave
{
    public static class SlaveLogManger
    {
        private static string sLoggerName = ConfigurationManager.AppSettings["SlaveLoggerName"].ToString();
        public static List<TinctTaskInfo> GetSlaveTaskInfoByLog(int lastDayStep)
        {
            var logger = TinctLogManager.Current.GetSLogger();
            List<TinctTaskInfo> lists = new List<TinctTaskInfo>();
            string path = logger.GetLoggerPath(sLoggerName);
            string fileName = path + "\\" + "task";

            for (int i = lastDayStep; i >= 0; i--)
            {
                string desfileName = fileName + (DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + (DateTime.Now.Day - i)) + ".json";
                if (File.Exists(desfileName))
                {
                    using (StreamReader reader = new StreamReader(File.OpenRead(desfileName)))
                    {
                        while (true)
                        {
                            var infostr = reader.ReadLine();
                            if (string.IsNullOrEmpty(infostr)) { break; }
                            var entity = LogEntity.GetObjectBySerializeString(infostr);
                            var taskinfo = TinctTaskInfo.GetObjectBySerializeString(entity.Message.ToString());
                            lists.Add(taskinfo);
                        }
                    }


                }


            }
            return lists;
        }

        public static void LogTaskinfo(TinctTaskInfo taskInfo)
        {
            ILogger logger = TinctLogManager.Current.GetSLogger();
            LogEntity entity = new LogEntity();
            entity.Message = taskInfo.ToJsonSerializeString();
            logger.LogInfo(entity);
        }

    }
}
