using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tinct.Net.Message.Node;
using System.Net;
using Tinct.Net.Communication.Node;


namespace Tinct.Net.Communication.UnitTest.Machine
{
    [TestClass]
    public class MachineRepositoriesTest1
    {

        [TestMethod]
        public void UpdateMachineInfoTest()
        {
            //NodeRepository macrepos = new NodeRepository();

            //NodeInfo machineMessage = new NodeInfo();
            //machineMessage.NodeName = Dns.GetHostName();
            //machineMessage.LastUpdateTime = DateTime.Now;
            //NodeInvokeInfo macin = new NodeInvokeInfo();
            //macin.ActionName = "TestAction";
            //macin.ControllerName = "TestController";
            //macin.Status = NodeStatus.Running;
            //macin.TaskID = new Guid();

            //machineMessage.NodeInvokeInfoList.Add(macin);
            //bool result1 = macrepos.UpdateNodeInfo(machineMessage);

            //NodeInvokeInfo macin2 = new NodeInvokeInfo();
            //macin2.ActionName = "TestAction";
            //macin2.ControllerName = "TestController";
            //macin2.Status = NodeStatus.Running;
            //macin2.TaskID = new Guid();
            //machineMessage.NodeInvokeInfoList.Add(macin);

            //bool result2 = macrepos.UpdateNodeInfo(machineMessage);

            //Assert.IsTrue(result1 && result2);

        }

        [TestMethod]
        public void UpdateMachineInfoByStringTest()
        {

            //NodeRepository macrepos = new NodeRepository();

            //NodeInfo machineMessage = new NodeInfo();
            //machineMessage.NodeName = Dns.GetHostName();
            //machineMessage.LastUpdateTime = DateTime.Now;
            //NodeInvokeInfo macin = new NodeInvokeInfo();
            //macin.ActionName = "TestAction";
            //macin.ControllerName = "TestController";
            //macin.Status = NodeStatus.Running;
            //macin.TaskID = new Guid();

            //machineMessage.NodeInvokeInfoList.Add(macin);
            //bool result1 = macrepos.UpdateNodeInfo(machineMessage.ToJsonSerializeString());

            //NodeInvokeInfo macin2 = new NodeInvokeInfo();
            //macin2.ActionName = "TestAction";
            //macin2.ControllerName = "TestController";
            //macin2.Status = NodeStatus.Running;
            //macin2.TaskID = new Guid();
            //machineMessage.NodeInvokeInfoList.Add(macin);

            //bool result2 = macrepos.UpdateNodeInfo(machineMessage.ToJsonSerializeString());

            //string message = "11111";
            //bool result3 = macrepos.UpdateNodeInfo(message);

            //Assert.IsTrue(result1 && result2 && !result3);

        }
        [TestMethod]
        public void GetAvaiableMachineNameTest()
        {
            //NodeRepository macrepos = new NodeRepository();

            //NodeInfo machineMessage = new NodeInfo();
            //machineMessage.NodeName = Dns.GetHostName();
            //machineMessage.LastUpdateTime = DateTime.Now;
            //NodeInvokeInfo macin = new NodeInvokeInfo();
            //macin.ActionName = "TestAction";
            //macin.ControllerName = "TestController";
            //macin.Status = NodeStatus.Running;
            //macin.TaskID = new Guid();

            //machineMessage.NodeInvokeInfoList.Add(macin);
            //bool result1 = macrepos.UpdateNodeInfo(machineMessage);

            //string controllerName = "TestController";
            //string actionName = "TestAction";

            //string str = macrepos.GetAvaiableNodeName(controllerName, actionName);
            //bool result2 = str == Dns.GetHostName() ? true : false;

            //controllerName = "TestController2";
            //actionName = "TestAction2";
            //str = macrepos.GetAvaiableNodeName(controllerName, actionName);
            //bool result3 = str == Dns.GetHostName() ? true : false;

            //Assert.IsTrue(result1 && !result2 && result3);

        }
        [TestMethod]
        public void CheckUnConnectMachineTest()
        {
            //NodeRepository macrepos = new NodeRepository();

            //NodeInfo machineMessage = new NodeInfo();
            //machineMessage.NodeName = Dns.GetHostName();
            //machineMessage.LastUpdateTime = DateTime.Now;
            //NodeInvokeInfo macin = new NodeInvokeInfo();
            //macin.ActionName = "TestAction";
            //macin.ControllerName = "TestController";
            //macin.Status = NodeStatus.Running;
            //macin.TaskID = new Guid();
            //macrepos.NodeInfoList.Add(machineMessage);

            //macrepos.CheckUnConnectNode();
            //System.Threading.Thread.Sleep(50000);
            //Assert.IsTrue(macrepos.NodeInfoList.Count == 0);

        }


    }
}
