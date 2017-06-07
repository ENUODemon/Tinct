using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tinct.TinctTaskMangement.Util;
using Tinct.TinctTaskMangement;
using System.Threading;
using Tinct.Net.Message.Message;
using Tinct.Net.Communication.Slave;
using Tinct.Net.Communication.Cfg;
using Tinct.Net.Communication.Master;

namespace Tinct.TInctTaskMangement.UnitTest.Util
{
    /// <summary>
    /// Summary description for FileTaskTest
    /// </summary>
    [TestClass]
    public class FileTaskTest
    {
        public FileTaskTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void RunFileTask()
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

            q.QueueTinctTask(t1);

            FileTask file = new FileTask();
            file.FileName = "Tinct.PlatformController.dll";
            file.SourcePath = "test.txt";
            file.Content = new List<byte>();


            Thread.Sleep(10000);

            TinctTaskService.DeployFile(file);
            //TinctMessage msg = new TinctMessage();
            //msg.MessageHeader = new MessageHeader();
            //msg.MessageHeader.CommandType = CommandType.Deploy;
            //msg.MessageBody = new MessageBody();
            //msg.MessageBody.Datas = file.ToJsonSerializeString();
            //TinctMasterNode.Current.SendMessage(TinctNodeCongratulations.MasterName, msg);

            Console.Read();
        }
    }
}
