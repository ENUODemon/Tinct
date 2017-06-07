using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tinct.TinctTaskMangement.Util;
using Tinct.Net.Communication.Slave;
using System.Threading;
using Tinct.Net.Communication.Master;
using Tinct.Net.Message.Message;
using Tinct.TinctTaskMangement.Handler;
using Tinct.Common.Log;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement;

namespace Tinct.TInctTaskMangement.UnitTest.Util
{
    [TestClass]
    public class TinctTaskMangerTest
    {
        [TestMethod]
        public void StartTest()
        {


            TinctTaskRepository q = new TinctTaskRepository();
            var mloggname = "Masterlogger";
            var mfilename = "Log4net.config";
            var slogname = "Slavelogger";
            TinctTaskService.StartMasterService();       
         
            Thread.Sleep(2000);
            TinctTaskService.StartSlaveService(slogname, mfilename);
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
           

            Thread.Sleep(12000);

           

            Assert.IsTrue(t2.Status==TinctTaskStatus.Completed);
          
        }
    }
}
