using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace Tinct.Common.Log
{
    public  class TinctLogManager
    {

      
     
        private string logVirtualpath = ConfigurationManager.AppSettings["VirtualLogPath"].ToString();

        private ILogger Mlogger;
        private ILogger Slogger;

        static TinctLogManager() 
        {
            Current = new TinctLogManager();
        }
        public static TinctLogManager Current { get; set; }

        public TinctLogManager() 
        {
            Current = this;
        }

        public ILogger GetMLogger() 
        {
            string mLoggerName = ConfigurationManager.AppSettings["MasterLoggerName"].ToString();
           // if (Mlogger == null) 
          //  {
                  Mlogger = new TinctLogger(mLoggerName, logVirtualpath);
          //  }

            return Mlogger;
        
        }
        public ILogger GetSLogger()
        {
            string sLoggerName = ConfigurationManager.AppSettings["SlaveLoggerName"].ToString();
            if (Slogger == null)
            {
                Slogger = new TinctLogger(sLoggerName, logVirtualpath);
            }

            return Slogger;

        }



      

        //public List<string> GetSlaveLogFileConent(int lastDayStep)
        //{
        //    List<string> lists = new List<string>();
        //    string path = localLogger.GetSlaveLoggerPath();
        //    string fileName = path + "\\" + "task";

        //    for (int i = lastDayStep; i >= 0; i--)
        //    {
        //        string desfileName = fileName + (DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + (DateTime.Now.Day - i)) + ".json";
        //        if (File.Exists(desfileName))
        //        {
        //            lists.AddRange(File.ReadLines(desfileName));
        //        }


        //    }
        //    return lists;
        //}


    }
}
