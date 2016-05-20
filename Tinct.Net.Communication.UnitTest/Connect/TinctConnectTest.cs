using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tinct.Net.Communication.Connect;
using System.Configuration;
using System.Net;
namespace Tinct.Net.Communication.UnitTest.Connect
{
    [TestClass]
    public class TinctConnectTest
    {
        private int masterPort = int.Parse(ConfigurationManager.AppSettings["MasterPort"]);
        private int slaveport = int.Parse(ConfigurationManager.AppSettings["SlavePort"]);

        [TestMethod]
        public void StartServerTest()
        {
            TinctConnect tincon = new TinctConnect();
            bool result = tincon.ListenningPort(masterPort);
            System.Threading.Thread.Sleep(3000);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void SendSendMessageTest()
        {
            TinctConnect tincon = new TinctConnect();
            if (tincon.ListenningPort(masterPort))
            {
                bool result = tincon.SendMessage(Dns.GetHostName(), masterPort, "hellworld");
                Assert.IsTrue(result);
            }
            else
            {
                Assert.Fail();
            }
        }

        public void EndServerTest()
        {
            TinctConnect tincon = new TinctConnect();
            bool result = tincon.StopListenning();
            Assert.IsTrue(result);
        }


    }
}
