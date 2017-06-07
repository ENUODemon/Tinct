using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Communication.Node;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Node;
using Tinct.TinctTaskMangement.Util;

namespace Tinct.Net.Communication.Master
{
    public class MDeployMessageHandler : IMessageHandler
    {

        public bool HanderMessage(TinctMessage message)
        {
            var deploycontent = DeployContent.GetObjectBySerializeString(message.MessageBody);
            if (deploycontent.FileName == null)
            {
                return true;
            }
            else 
            {
                var node = NodeRepository.Current.NodeInfoList.FirstOrDefault(t => t.NodeName == deploycontent.TargetNodeName);
                if (deploycontent.Status == "Failed")
                {
                    node.Status = NodeStatus.DeployFailed;
                }
                else 
                {
                    node.Status = NodeStatus.Running;
                }
            }

            return false;
        }
    }
}
