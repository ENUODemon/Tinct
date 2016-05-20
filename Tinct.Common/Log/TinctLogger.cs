using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using log4net.Appender;
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"Log\Log4net.config", Watch = true)]
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"..\..\Log\Log4net.config", Watch = true)]

namespace Tinct.Common.Log
{
    public class TinctLogger : ILogger
    {
        private ILog logger;
        public TinctLogger(string loggerName,string logVirtualpath)
        {
            
            //var collection= log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory +@"Log\Log4net.config"));
            try
            {
                var logfileinfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + logVirtualpath);
                log4net.Config.XmlConfigurator.Configure(logfileinfo);
                logger = LogManager.GetLogger(loggerName);
            }
            catch 
            {
                throw;
            }
        }

        public void LogInfo(LogEntity logEntity)
        {
           
          
            string message = "";
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StringWriter sw = new StringWriter())
                {
                    serializer.Serialize(sw, logEntity);
                    message = sw.GetStringBuilder().ToString();
                }
            }
            catch 
            {
                throw;
            }

            logger.Info(message);
        }

        public void LogMessage(string message)
        {
            logger.Info(message);
        }

        public string GetLoggerPath(string loggerName)
        {
            string result = "";
            // var appender = LogManager.GetRepository().GetAppenders().FirstOrDefault(t=>t.Name=="TasklogInfo") as RollingFileAppender;
            var appender = LogManager.GetRepository().GetAppenders().FirstOrDefault(t => t.Name == loggerName) as RollingFileAppender;

            if (appender != null)
            {
                result = Path.GetDirectoryName(appender.File);
            }
            return result;
        }
    }
}
