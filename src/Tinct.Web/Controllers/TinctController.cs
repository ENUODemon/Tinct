using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tinct.TinctTaskMangement;
using Tinct.TaskExcution.Util;
using System.Threading;
using System.IO;

namespace Tinct.Web.Controllers
{
    public class TinctController : Controller
    {

        public IActionResult StartTask()
        {
            TinctTaskRepository q = new TinctTaskRepository();
            var mloggname = "Masterlogger";
            var mfilename = "Log4net.config";
            TinctTaskService.StartMasterService();

       
            Thread.Sleep(2000);
            TinctTaskService.StartTaskService(q, mloggname, mfilename);

            TinctTask t1 = new TinctTask();
            t1.Name = "test";
            t1.DllName = "Tinct.PlatformController";
            t1.NamespaceName = "Tinct.PlatformController";
            t1.ClassName = "TinctTestController";
            t1.MethodName = "LoadData1";
            t1.Datas = "test";

            TinctTask t2 = new TinctTask();
            t2.Name = "test";
            t2.DllName = "Tinct.PlatformController";
            t2.NamespaceName = "Tinct.PlatformController";
            t2.ClassName = "TinctTest1Controller";
            t2.MethodName = "LoadData1";
            t2.Datas = "test";

            q.QueueTinctTask(t1);
            q.QueueTinctTask(t2);

            return View("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logs()
        {
            return View();
        }

        public IActionResult Tasks()
        {

            Newtonsoft.Json.JsonSerializer js = new Newtonsoft.Json.JsonSerializer();
            StringWriter sw = new StringWriter();

            js.Serialize(sw, TinctTaskService.GetCurrentTasks());

            ViewBag.Tasks= sw.GetStringBuilder().ToString();
            ViewBag.PageSize = 10;
            return View();
        }

        public IActionResult Nodes()
        {
            return View();
        }
    }
}