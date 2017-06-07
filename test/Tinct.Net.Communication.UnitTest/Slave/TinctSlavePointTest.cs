using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tinct.Net.Communication.Slave;
using System.Net;
using Tinct.Net.Communication.Master;
using Tinct.Net.Message.Message;
using Tinct.Common.Extension;
using System.Threading;

namespace Tinct.Net.Communication.UnitTest.Slave
{
    [TestClass]
    public class TinctSlavePointTest
    {
      
        [TestMethod]
        public void StartSlaveTest()
        {
            TinctSlaveNode slavePoint = new TinctSlaveNode();
            bool result = slavePoint.StartSlave();
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void ReceviceMessageTest()
        {
            TinctSlaveNode slavePoint = new TinctSlaveNode();
            var handler = new TestMessageHandler();
            slavePoint.MessageHandlers.Add(handler);
            bool result = slavePoint.StartSlave();

            TinctMessage msg = new TinctMessage();
            msg.MessageBody = new MessageBody();
            msg.MessageBody.Datas = "test";

            TinctMasterNode sendPoint = new TinctMasterNode();
            sendPoint.SendMessage(slavePoint.NodeInfo.NodeName, msg);

            Thread.Sleep(1000);


            //msg.MessageBody = "test1";
            sendPoint.SendMessage(slavePoint.NodeInfo.NodeName, msg);
            Assert.IsTrue(handler.Message == "test");

        }


    }
}
