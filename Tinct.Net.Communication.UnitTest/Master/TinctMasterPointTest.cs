using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tinct.Net.Communication.Master;
using System.Net;
using Tinct.Net.Communication.Slave;

namespace Tinct.Net.Communication.UnitTest.Master
{
    [TestClass]
   public class TinctMasterPointTest
    {
        [TestMethod]
        public void SendMessageToSlaveTest()
        {
            TinctMasterNode masterPoint = new TinctMasterNode();
            TinctSlaveNode slavePoint = new TinctSlaveNode();
            slavePoint.StartSlave();
            bool result=masterPoint.SendMessageToSlave("hello", Dns.GetHostName());
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void StartMasterTest()
        {
            TinctMasterNode masterPoint = new TinctMasterNode();
            bool result= masterPoint.StartMaster();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EndMasterTest()
        {
            TinctMasterNode masterPoint = new TinctMasterNode();
            bool result = masterPoint.EndMaster();
            Assert.IsTrue(result);
        }
    }
}
