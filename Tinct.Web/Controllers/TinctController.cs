using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tinct.Net.Message.Node;
using Tinct.TinctTaskMangement;

using Tinct.TinctTaskMangement.TinctWork;
using Tinct.Web.Models.LogModel;
using Tinct.Web.TinctTaskM;
using System.Configuration;
using Tinct.Common.Log;
using Tinct.Net.Message.Task;
using Tinct.Net.Communication.Node;
using Tinct.TinctTaskMangement.Util;
using Tinct.Net.MasterPointConsole;

namespace Tinct.Web.Controllers
{
    public class TinctController : Controller
    {

        static bool startServer = false;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StartTask()
        {
            if (!startServer)
            {
                startServer = true;
                new Task(() =>
                {
                    TinctTaskBridge.TaskManger.Start();
                }).Start();

                //GitHubTasks github = new GitHubTasks();
                //github.GennerateTinctTasks();
                //MeetupTasks meetup = new MeetupTasks();
                //meetup.GennerateTinctTasks();

            }
            return View("Index");
        }
        public ActionResult RunCertainTask(string platform)
        {
            switch (platform.Trim().ToLower())
            {
                case "meetup":
                    MeetupTasks meetup = new MeetupTasks();
                    meetup.GennerateTinctTasks();
                    break;
                case "github":
                    GitHubTasks github = new GitHubTasks();
                    github.GennerateTinctTasks();
                    break;
                default:
                    break;
            }




            return View("Index");
        }

        public ActionResult Create()
        {

            return View();
        }

        public ActionResult CreateTask()
        {
            TinctTask t = new TinctTask();
            t.Name = Request.Form["Name"];
            t.Context.TaskData = Request.Form["Data"];
            t.Context.ControllerName = Request.Form["Controller"];
            t.Context.ActionName = Request.Form["Action"];


            switch (Request.Form["Priority"])
            {
                case "Low":
                    t.Priority = TaskPriority.Low; break;
                case "Medium":
                    t.Priority = TaskPriority.Medium; break;
                case "High":
                    t.Priority = TaskPriority.High; break;

            }
            switch (Request.Form["Status"])
            {
                case "WaittingToRun":
                    t.Status = TinctTaskStatus.WaittingToRun;
                    break;
                case "Waitting":
                    t.Status = TinctTaskStatus.Waitting;
                    break;
            }


            return View("Index");

        }


        public ActionResult Nodes(string nodeName)
        {


            var nodeInfosLists = NodeRepository.Current.NodeInfoList;
            if (nodeName == null)
            {
                ViewBag.Title = "Nodes";
                ViewBag.NodeInfosLists = nodeInfosLists;

            }
            else
            {
                ViewBag.Title = nodeName;
                ViewBag.NodeInfosLists = nodeInfosLists.Select(item => item.NodeName == nodeName);
            }

            return View();
        }

        public ActionResult Tasks(string id)
        {


            var taskInfosLists = TinctTaskBridge.TaskManger.TaskRepository.GetCurrentTinctTasks();
            if (id == null)
            {
                ViewBag.Title = "Nodes";
                ViewBag.TaskInfosLists = taskInfosLists;

            }
            else
            {
                ViewBag.Title = id;
                ViewBag.TaskInfosLists = taskInfosLists.Select(item => item.ID == new Guid(id));
            }

            return View();

        }

        public ActionResult UpLoadTasks()
        {
            HttpPostedFileBase postfile = Request.Files["file"];
            StreamReader sr = new StreamReader(postfile.InputStream);


            Newtonsoft.Json.JsonSerializer jss = new Newtonsoft.Json.JsonSerializer();


            var tasklists = jss.Deserialize<List<ConfigTinctTask>>(new Newtonsoft.Json.JsonTextReader(sr));
            foreach (var task in tasklists)
            {
                // TinctTaskBridge.taskRepos.AddTinctTask(task.ConvertToTinctTask());
            }

            return View("Index");
        }

        public ActionResult Logs()
        {
            string datestr = Request.Form["LogDate"];
            string taskID = Request.Form["TaskID"];
            string path = @"C:/TinctLog/TaskLog/";

            string fileName = "task" + datestr + ".json"; //20160105

            IEnumerable<string> logLists;
            List<LogEntity> logs = new List<LogEntity>();

            if (!string.IsNullOrEmpty(datestr))
            {
                try
                {
                    logLists = System.IO.File.ReadLines(path + fileName);
                }
                catch
                {
                    logs = new List<LogEntity>();
                    ViewBag.Logs = logs;
                    return View("Logs");
                }
            }
            else
            {
                logLists = new List<string>();
            }

            Newtonsoft.Json.JsonSerializer jss = new Newtonsoft.Json.JsonSerializer();
            TextReader tw;

            foreach (var loginfo in logLists)
            {
                tw = new StringReader(loginfo);
                LogEntity loge = jss.Deserialize<LogEntity>(new Newtonsoft.Json.JsonTextReader(tw));
                StringReader sr = new System.IO.StringReader(loge.Message.ToString());
                TinctTaskInfo taskingo = jss.Deserialize<TinctTaskInfo>(new Newtonsoft.Json.JsonTextReader(sr));
                loge.Message = taskingo;
                logs.Add(loge);

            }

            if (!string.IsNullOrEmpty(taskID))
            {
                try
                {
                    logs = logs.Where(l => ((TinctTaskInfo)l.Message).ID == new Guid(taskID)).ToList();
                }
                catch
                {
                    logs = new List<LogEntity>();
                }
            }
            ViewBag.Logs = logs;

            return View("Logs");

        }


        public ActionResult Deploy()
        {
            return View("Deploy");
        }

        public ActionResult DeployFile()
        {
            HttpPostedFileBase postfile = Request.Files["file"];
            StreamReader sr = new StreamReader(postfile.InputStream);


            DeployContent content = new DeployContent();
            content.FilePath = @"D:\MyLearn\Source\Repos\TinctV2-FrameWork\Tinct.PlatformInvoke\bin\change";
            content.FileName = "Tinct.PlatformController.dll";
            DeployTask dtask = new DeployTask();
            dtask.Content = content;
            dtask.Start();




            return View("Index");
        }

    }
}