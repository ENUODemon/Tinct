using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;
using System.Configuration;
using System.IO;
using Tinct.Net.Communication.Node;
using Tinct.Net.Message.Message;
using Tinct.Common.Extension;

namespace Tinct.Net.Communication.Master
{
    public class TinctMasterNode : TinctNode
    {

        private int masterPort = int.Parse(ConfigurationManager.AppSettings["MasterPort"]);
        private int slaveport = int.Parse(ConfigurationManager.AppSettings["SlavePort"]);
       
        public static TinctMasterNode Current { get; private set; }

        public List<NodeInfo> SlaveNodes = new List<NodeInfo>();

        static TinctMasterNode()
        {
            Current = new TinctMasterNode();
        }

        public TinctMasterNode()
        {
            Current = this;
        }

        public TinctMasterNode(IConnect newconnect)
        {
            connect = newconnect;
            Current = this;
        }

        public bool SendMessage(string machineName, TinctMessage message)
        {
            PackageMessage pmsg = new PackageMessage();
            pmsg.SourceName = NodeInfo.NodeName;
            pmsg.DestinationName = machineName;
            pmsg.SendTimeStamp= DateTimeExtension.GetTimeStamp();
            pmsg.Message = message;
            return SendMessage(machineName, slaveport, pmsg);
        }

        public bool StartMaster()
        {
            RaisePackageMessage = new Action<PackageMessage>((msg) => { UpdateSlaveNodeInfo(msg); });
            StartReceviceMessageService(masterPort);
            HandleReceviceMessage();
            return true;
        }

        private void UpdateSlaveNodeInfo(PackageMessage msg)
        {
            var nodeInfo = SlaveNodes.FirstOrDefault(node => node.NodeName == msg.SourceName);
            if (nodeInfo == null)
            {
                NodeInfo newnodeInfo = new NodeInfo();
                newnodeInfo.LastUpdateTime = msg.ReceviceTimeStamp;
                newnodeInfo.Status = NodeStatus.Running;
                newnodeInfo.NodeName = msg.SourceName;
                SlaveNodes.Add(newnodeInfo);
            }
            else
            {
                nodeInfo.LastUpdateTime = msg.ReceviceTimeStamp;
                nodeInfo.Status = NodeStatus.Running;
                nodeInfo.NodeName = msg.SourceName;
            }

        }

        public bool EndMaster()
        {
            CloseConnectResource();
            return true;
        }

    }
}
