using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tinct.TinctTaskMangement.TinctWork;
using Tinct.Net.Message;
using System.Net;
using Tinct.Net.Message.Node;
using Tinct.Net.Message.Task;

namespace Tinct.TInctTaskMangement.UnitTest.Util
{
    [TestClass]
    public class TinctTaskRepositoryTest
    {
        [TestMethod]
        public void AddTinctTaskTest()
        {
        //    TinctTaskRepository repos = new TinctTaskRepository();
        //    try
        //    {
        //        repos.AddTinctTask(null);
        //    }
        //    catch
        //    {
        //        Assert.IsTrue(true);
        //    }


        }

        [TestMethod]
        public void GetAvailableTinctTaskTest()
        {
            //TinctTaskRepository repos = new TinctTaskRepository();
            //bool result1 = repos.GetAvailableTinctTask() == null ? true : false;
            //repos.AddTinctTask(new TinctTask());
            //bool result2 = repos.GetAvailableTinctTask() == null ? true : false;
            //Assert.IsTrue(result1 && !result2);
        }

        [TestMethod]
        public void GetTinctTaskByIDTest()
        {
            //TinctTaskRepository repos = new TinctTaskRepository();
            //TinctTask task = new TinctTask();
            //repos.AddTinctTask(task);
            //var resulttask = repos.GetTinctTaskByID(task.ID);
            //Assert.IsNotNull(resulttask);
        }
        [TestMethod]
        public void RemoveTinctTaskTest()
        {
            //TinctTaskRepository repos = new TinctTaskRepository();
            //TinctTask task = new TinctTask();
            //bool result1 = repos.RemoveTinctTask(task);
            //repos.AddTinctTask(task);
            //bool result2 = repos.RemoveTinctTask(task);

          //  Assert.IsTrue(!result1 && result2);
        }
        [TestMethod]
        public  void UpdateTinctTasksStatusTest()
        {
            //TinctTaskRepository repos = new TinctTaskRepository();
            //TinctTask tincttask = new TinctTask();
            //repos.AddTinctTask(tincttask);
            //TaskMessage message = new TaskMessage();
            //message.TaskID = tincttask.ID;
            //message.MachineName = Dns.GetHostName();
            //message.TaskData = "MyTestData";
            //message.MachineInvokeStatus = NodeStatus.Completed;
            //repos.UpdateTinctTasksStatus(message.ToJsonSerializeString());
            //Assert.IsTrue(tincttask.TaskStatus == TinctTaskStatus.Completed);

        }


    }
}
