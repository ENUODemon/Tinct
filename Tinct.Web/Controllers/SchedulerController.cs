using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tinct.Web.Helper;
using Tinct.Web.Models;

namespace Tinct.Web.Controllers
{
    public class SchedulerController : Controller
    {
        private object thisLock = new object();
        public static List<PlatformSetting> plist = new List<PlatformSetting>();
        //
        // GET: /Scheduler/
        public ActionResult Index()
        {
            ViewBag.PlatformSettingList = plist;
            return View("Index");
        }
        public void RunTask(string taskName)
        {
            PlatformSetting toBeRunItem = plist.SingleOrDefault(p => p.Name == taskName);
            toBeRunItem.RaiseCreatTaskEvent();
            toBeRunItem.Status = "running";
        }
        public void End()
        {
        }
        public bool DeleteTask(string taskName)
        {
            bool returnVal;
            PlatformSetting toBeRemoveItem=plist.SingleOrDefault(p=>p.Name==taskName);
            lock (thisLock)
            {
                returnVal=plist.Remove(toBeRemoveItem);
            }
            return returnVal;         
        }
        public JsonResult SetScheduler(string datas)
        {
            PlatformSetting plat;
            JsonSerializer serializer = new JsonSerializer();
            StringReader reader = new StringReader(datas);
            try
            {
                plat = serializer.Deserialize<PlatformSetting>(new JsonTextReader(reader));
            }
            catch (Exception e)
            {
                throw e;
            }
            if (plist.Where(p => p.Name == plat.Name).ToList().Count > 0)
            {
                return Json(false);
            }    
            //plat.Name = "Test";
            //plat.PlatformName = "Test";
            //plat.CreatedTime = DateTime.Now;
            //plat.ExactRunTime = DateTime.Now.AddMinutes(1);
            //plat.NextRunTime = plat.ExactRunTime;
            //plat.Interval = 1;
            plat.CreatedTime = DateTime.Now;
            plat.CreateTaskEvent += PlatformCreatTask;              
            lock (thisLock)
            {
                plist.Add(plat);
            }
            return Json(true);
        }

        public void StartTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 1000;//执行间隔时间,单位为毫秒  
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);
        }
        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var platformSetting in plist)
            {
                platformSetting.ExactRunTime = platformSetting.NextRunTime;
                if (e.SignalTime.ToLongTimeString() == platformSetting.ExactRunTime.Value.ToLongTimeString())
                {
                    platformSetting.LastRunTime = e.SignalTime;
                    platformSetting.NextRunTime = e.SignalTime.AddDays(platformSetting.Interval);
                    platformSetting.RaiseCreatTaskEvent();
                }
            }
        }

        private void PlatformCreatTask(object sender, PlatformEventArgs e)
        {
            PlatformSetting tempObj = (PlatformSetting)sender;
            if (tempObj.PlatformName == "Meetup")
            {
                Console.WriteLine("start meetup");
            }
            else if (tempObj.PlatformName == "Test")
            {
                TestTasks test = new TestTasks();
                test.GennerateTinctTasks();
            }
        }
	}
}