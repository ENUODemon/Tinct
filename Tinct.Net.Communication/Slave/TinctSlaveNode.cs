using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Node;
using Tinct.Net.MessageDispath.Route;
using System.Configuration;
using System.Net;
using Tinct.Common.Log;
using Newtonsoft.Json;
using System.IO;
using Tinct.Net.Message.Task;
using Tinct.Net.Message.Message;



namespace Tinct.Net.Communication.Slave
{
    public class TinctSlaveNode : SlaveNode
    {
        private object synchandlMessage = new object();
        private object sysncNodeInfoObject = new object();
        private int masterPort = int.Parse(ConfigurationManager.AppSettings["MasterPort"].ToString());
        private int slavePort = int.Parse(ConfigurationManager.AppSettings["slavePort"].ToString());
        private string masterName = ConfigurationManager.AppSettings["Master"].ToString();
        private System.Threading.Timer blueTimer;


        public List<IMessageHandler> MessageHandlers { get; set; }
        public static TinctSlaveNode Current { get; private set; }
        public NodeInfo SlaveNodeInfo { get; set; }


        static TinctSlaveNode()
        {
            Current = new TinctSlaveNode();
            
        }

        public TinctSlaveNode()
        {

            SlaveNodeInfo = new NodeInfo();
            SlaveNodeInfo.NodeName = Dns.GetHostName();
            MessageHandlers = new List<IMessageHandler>();
            tinctCon = new TinctConnect();

            Current = this;
        }
        public TinctSlaveNode(ITinctConnect tinctCon)
        {
            SlaveNodeInfo = new NodeInfo();
            SlaveNodeInfo.NodeName = Dns.GetHostName();
            MessageHandlers = new List<IMessageHandler>();
            this.tinctCon = tinctCon;
            Current = this;
        }

        public bool SendMessageToMaster(string message, string machineName)
        {

            return tinctCon.SendMessage(machineName, masterPort, message);

        }

        public void AddNodeTaskInfo(TinctTaskInfo taskInfo)
        {
            lock (sysncNodeInfoObject)
            {
                SlaveLogManger.LogTaskinfo(taskInfo);
                SlaveNodeInfo.TinctTaskInfoList.Add(taskInfo);
            }
        }
        public void RemoveNodeTaskInfo(TinctTaskInfo taskInfo)
        {
            lock (sysncNodeInfoObject)
            {
                SlaveLogManger.LogTaskinfo(taskInfo);

                var index = SlaveNodeInfo.TinctTaskInfoList.FindIndex(t => t.ID == taskInfo.ID);
                if (index != -1)
                {
                    SlaveNodeInfo.TinctTaskInfoList.RemoveAt(index);
                }
            }
        }

        public void StartSyncNodeInfoService()
        {
            blueTimer = new System.Threading.Timer
               (
                   (con) =>
                   {
                       string message = "";
                       lock (sysncNodeInfoObject)
                       {
                           SlaveNodeInfo.LastUpdateTime = DateTime.Now;
                           message = SlaveNodeInfo.ToJsonSerializeString();
                       }
                       (con as TinctConnect).SendMessage(masterName, masterPort, message);
                   },
                   tinctCon, 1000, 10000
               );
        }


        public override bool StartSlave()
        {



            tinctCon.TaskMessage += new EventHandler<ReceiveMessageArgs>(ReciveConMessage);
            IMessageHandler deployHandler = new DeployMessageHandler();
            IMessageHandler messageHandler = new SMessageHandler();
            MessageHandlers.Add(deployHandler);
            MessageHandlers.Add(messageHandler);




            StartSyncNodeInfoService();
            if (tinctCon.ListenningPort(slavePort))
            {
                Console.WriteLine("Start Slave Complete!");
                return true;

            }
            else { return false; }

        }

        public override bool EndSlave()
        {
            if (tinctCon != null)
            {
                return tinctCon.StopListenning();
            }
            return true;
        }


        private void ReciveConMessage(object sender, ReceiveMessageArgs e)
        {

            try
            {
                foreach (var handler in MessageHandlers)
                {
                    if(!handler.HanderMessage(e.ReceivedMessage))
                    {
                        break;
                    }
                }
            }
            catch
            {

            }

        }

    }
}
