using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Message;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.TinctWork;
using Tinct.TinctTaskMangement.Util;
using System.Threading;

namespace Tinct.Net.MasterPointConsole
{
    class Program
    {
        static ITinctTaskRepository tre = new TinctTaskRepository();
        static void Main(string[] args)
        {
            TinctTaskMangement.TinctWork.TinctTaskManeger tm = new TinctTaskMangement.TinctWork.TinctTaskManeger();
            tm.TaskRepository = tre;
            new Task(() => { tm.Start(); }).Start();

            //CrawlDataTasks t = new CrawlDataTasks();
            //t.GennerateTinctTaskManeger();


            //TinctConnect c = new TinctConnect();
            //c.ListenningPort(8885);
            //c.TaskMessage += C_TaskMessage;

            TinctTask t1 = new TinctTask();
            t1.Priority = TaskPriority.Low;
            t1.Name = "TestTask1";
            t1.Context.TaskData = "";
            t1.Context.ControllerName = "TinctTest1";
            t1.Context.ActionName = "LoadData1";
            t1.Status = TinctTaskStatus.WaittingToRun;

            t1.TinctTaskCompleted += t_TinctTaskCompleted;

            Thread.Sleep(10000);
            t1.Cancel();

            //TinctTask t2 = new TinctTask();
            //t2.Priority = TaskPriority.Low;
            //t2.Name = "TestTask2";
            //t2.Context.TaskData = "";
            //t2.Context.ControllerName = "TinctTest1";
            //t2.Context.ActionName = "LoadData1";
            //t2.Status = TinctTaskStatus.WaittingToRun;



            //TinctTask t3 = new TinctTask();
            //t3.Priority = TaskPriority.Low;
            //t3.Name = "TestTask3";
            //t3.Context.TaskData = "";
            //t3.Context.ControllerName = "TinctTest";
            //t3.Context.ActionName = "LoadData1";
            //t3.Status = TinctTaskStatus.WaittingToRun;
         
           
            //TinctTask t = new TinctTask();
            //t.Name = "MediumTask";
            //t.Priority = TaskPriority.Medium;
            //t.TaskStatus = TinctTaskStatus.WaittingToRun;
            //t.Context.TaskData = "";
            //t.Context.ControllerName = "TinctTest";
            //t.Context.ActionName = "LoadData2";
            
            //t.Context.TaskID = t.ID;


            //TinctTask t2 = new TinctTask();
            //t2.Priority = TaskPriority.High;
            //t2.Name = "HighTask";
            //t2.TaskStatus = TinctTaskStatus.WaittingToRun;
            //t2.Context.TaskData = "";
            //t2.Context.ControllerName = "TinctTest";
            //t2.Context.ActionName = "LoadData";
            //t2.Context.TaskID = t2.ID;

            //TinctTask t3 = new TinctTask();
            //t3.Priority = TaskPriority.High;
            //t3.Name = "HighTask";
            //t3.TaskStatus = TinctTaskStatus.WaittingToRun;
            //t3.Context.TaskData = "";
            //t3.Context.ControllerName = "TinctTest";
            //t3.Context.ActionName = "LoadData";
            //t3.Context.TaskID = t3.ID;

            //TinctTask t4 = new TinctTask();
            //t4.Priority = TaskPriority.High;
            //t4.Name = "HighTask";
            //t4.TaskStatus = TinctTaskStatus.WaittingToRun;
            //t4.Context.TaskData = "";
            //t4.Context.ControllerName = "TinctTest";
            //t4.Context.ActionName = "LoadData";
            //t4.Context.TaskID = t4.ID;

            //TinctTask t5 = new TinctTask();
            //t5.Priority = TaskPriority.High;
            //t5.Name = "HighTask";
            //t5.TaskStatus = TinctTaskStatus.WaittingToRun;
            //t5.Context.TaskData = "";
            //t5.Context.ControllerName = "TinctTest";
            //t5.Context.ActionName = "LoadData";
            //t5.Context.TaskID = t5.ID;

            //TinctTask t6 = new TinctTask();
            //t6.Priority = TaskPriority.High;
            //t6.Name = "HighTask";
            //t6.TaskStatus = TinctTaskStatus.WaittingToRun;
            //t6.Context.TaskData = "";
            //t6.Context.ControllerName = "TinctTest";
            //t6.Context.ActionName = "LoadData";
            //t6.Context.TaskID = t6.ID;

           

            //TinctTaskMangement.TinctWork.TinctTaskManeger tm = new TinctTaskMangement.TinctWork.TinctTaskManeger();

            //tm.TaskRepos = tre;

            //tre.AddTinctTask(t);
            //tre.AddTinctTask(t1);
            //tre.AddTinctTask(t2);
           // t.TinctTaskCompleted+=t_TinctTaskCompleted;
            //new Task(() =>
            //{
                
            //    tre.AddTinctTask(t);
            //    //tre.AddTinctTask(t1);
            //    //tre.AddTinctTask(t2);
            //    //tre.AddTinctTask(t3);
            //    //tre.AddTinctTask(t4);
            //    //tre.AddTinctTask(t5);
            //    //tre.AddTinctTask(t6);
                
            //}).Start();

           // tm.Start();




            Console.Read();


        }

        private static void C_TaskMessage(object sender, ReceiveMessageArgs e)
        {
            var remoteTaskinfo = TinctTaskInfo.GetObjectBySerializeString(e.ReceivedMessage);
            if (remoteTaskinfo != null)
            {

            }
        }

        private static void t_TinctTaskCompleted(object sender, TinctTaskEventArgs e)
        {
            Console.WriteLine("Task complete!");
            for (int i = 0; i < 5; i++)
            {
                TinctTask t5 = new TinctTask();
                t5.Priority = TaskPriority.High;
                t5.Name = "HighTask1";

                t5.Context.TaskData = "";
                t5.Context.ControllerName = "TinctTest1";
                t5.Context.ActionName = "LoadData";
                t5.Status = TinctTaskStatus.WaittingToRun;
            }
            for (int i = 0; i < 5; i++)
            {
                TinctTask t5 = new TinctTask();
                t5.Priority = TaskPriority.High;
                t5.Name = "HighTask1";

                t5.Context.TaskData = "";
                t5.Context.ControllerName = "TinctTest1";
                t5.Context.ActionName = "LoadData2";
                t5.Status = TinctTaskStatus.WaittingToRun;
            }

            //TinctTask t6 = new TinctTask();
            //t6.Priority = TaskPriority.High;
            //t6.Name = "HighTask2";
            //t6.Context.TaskData = "";
            //t6.Context.ControllerName = "TinctTest1";
            //t6.Context.ActionName = "LoadData1";
            //t6.Status = TinctTaskStatus.WaittingToRun;

            //TinctTask t3 = new TinctTask();
            //t3.Priority = TaskPriority.High;
            //t3.Name = "HighTask3";
            //t3.Context.TaskData = "";
            //t3.Context.ControllerName = "TinctTest1";
            //t3.Context.ActionName = "LoadData1";
            //t3.Status = TinctTaskStatus.WaittingToRun;
            //TinctTask t4 = new TinctTask();
            //t4.Priority = TaskPriority.High;
            //t4.Name = "HighTask4";
            //t4.Context.TaskData = "";
            //t4.Context.ControllerName = "TinctTest1";
            //t4.Context.ActionName = "LoadData1";
            //t4.Status = TinctTaskStatus.WaittingToRun;

        }
    }
}
