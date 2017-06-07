using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;
using System.Configuration;
using System.Net;
using Tinct.Common.Log;
using Newtonsoft.Json;
using System.IO;
using Tinct.Net.Message.Message;
using Tinct.Net.Communication.Node;
using Tinct.Common.Extension;
using Tinct.Net.Communication.Cfg;

namespace Tinct.Net.Communication.Slave
{
    public class TinctSlaveNode : TinctNode
    {

        private int masterPort = TinctNodeCongratulations.MasterPort;
        private string masterName = TinctNodeCongratulations.MasterName;
        private int slavePort = TinctNodeCongratulations.SlavePort;

     
        public static TinctSlaveNode Current { get; private set; }

        public NodeInfo MasterNodeInfo { get; set; } = new NodeInfo()
        {
            LastUpdateTime =0, Status=NodeStatus.Closed,NodeName= TinctNodeCongratulations.MasterName
        };


        static TinctSlaveNode()
        {
            Current = new TinctSlaveNode();
        }

        public TinctSlaveNode()
        {
            Current = this;
        }

        public TinctSlaveNode(IConnect newconnect)
        {
            connect = newconnect;
            Current = this;
        }

        public  bool SendMessage(string machineName, TinctMessage message)
        {
            PackageMessage pmsg = new PackageMessage();
            pmsg.SourceName = NodeInfo.NodeName;
            pmsg.DestinationName = machineName;
            pmsg.SendTimeStamp = DateTimeExtension.GetTimeStamp();
            pmsg.Message = message;
            return SendMessage(machineName, masterPort, pmsg);
        }

        public  bool StartSlave()
        {
            RaisePackageMessage = new Action<PackageMessage>((msg) => { UpdateMasteNodeInfo(msg); }); 
            StartReceviceMessageService(slavePort);
            HandleReceviceMessage();
            return true;
        }

        public  bool EndSlave()
        {
            CloseConnectResource();
            return true;
        }

        private void UpdateMasteNodeInfo(PackageMessage message)
        {
            MasterNodeInfo.LastUpdateTime = message.ReceviceTimeStamp;
            MasterNodeInfo.Status = NodeStatus.Running;
        }

    }
}
