using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tinct.Net.Communication.Slave;
using System.Net;
using Tinct.Net.Communication.Master;

namespace Tinct.Net.Communication.UnitTest.Slave
{
    [TestClass]
    public class TinctSlavePointTest
    {
        [TestMethod]
        public void SendMessageToMasterTest()
        {
            TinctSlaveNode slavePoint = new TinctSlaveNode();
            TinctMasterNode masterPoint = new TinctMasterNode();
            masterPoint.StartMaster();
            bool result = slavePoint.SendMessageToMaster("hello", Dns.GetHostName());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StartSlaveTest()
        {
            TinctSlaveNode slavePoint = new TinctSlaveNode();
            bool result = slavePoint.StartSlave();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EndSlaveTest()
        {
            TinctSlaveNode slavePoint = new TinctSlaveNode();
            bool result = slavePoint.EndSlave();
            Assert.IsTrue(result);
        }

    }
}
