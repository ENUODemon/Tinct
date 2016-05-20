using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Common.Log
{
    public interface ILogger
    {
        void LogInfo(LogEntity log);

        void LogMessage(string message);


        string GetLoggerPath(string loggerName);


    }
}
