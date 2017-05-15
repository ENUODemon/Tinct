using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.MessageDispath.Controller;

namespace Tinct.PlatformController
{
    public  class TinctTestController: ControllerBase
    {

        public ActionResult LoadData1(string taskDatas)
        {
           // throw new Exception();
            UrlActionResult result = new UrlActionResult();
       
            System.Threading.Thread.Sleep(5000);
            
            result.RemainTaskData = "";
            Console.WriteLine("test tinct  tinct 5000!");
            return result;
        }

        public ActionResult LoadData2(string taskDatas)
        {
            UrlActionResult result = new UrlActionResult();

            System.Threading.Thread.Sleep(10000);

            result.RemainTaskData = "";
            Console.WriteLine("test tinct  tinct 70000!");
            return result;
        }

    }
}
