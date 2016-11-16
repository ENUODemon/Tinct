using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.MessageDispath.Controller;

namespace Tinct.PlatformController
{
    public class TinctTest1Controller : ControllerBase
    {
        public ActionResult LoadData1(string taskDatas)
        {
            UrlActionResult result = new UrlActionResult();

            System.Threading.Thread.Sleep(15000);
            result.RemainTaskData = "";
            Console.WriteLine("test tinct  15000");
            result.TaskResult = "complete!";
            return result;
        }

        public ActionResult LoadData(string taskDatas)
        {
            UrlActionResult result = new UrlActionResult();

            System.Threading.Thread.Sleep(1000);
            result.RemainTaskData = "";
            Console.WriteLine("test tinct  10000");
            return result;
        }

        public ActionResult LoadData2(string taskDatas)
        {
            UrlActionResult result = new UrlActionResult();

            System.Threading.Thread.Sleep(2000);
            result.RemainTaskData = "";
            Console.WriteLine("test tinct  20000");
            return result;
        }
    }
}
