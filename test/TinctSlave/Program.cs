using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement;

namespace TinctSlave
{
    class Program
    {
        static void Main(string[] args)
        {

            var mfilename = "Log4net.config";
            var slogname = "Slavelogger";
            TinctTaskService.StartSlaveService(slogname, mfilename);
            Console.Read();
        }
    }
}
