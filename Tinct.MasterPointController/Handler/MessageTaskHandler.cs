using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Communication.Node;
using Tinct.Net.Message.Node;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Handler
{
    public class MessageTaskHandler : IMessageHandler
    {

        public bool HanderMessage(string message)
        {

            if (message.StartsWith("{\"NodeName\""))
            {
                NodeInfo NodeInfo = NodeInfo.GetObjectBySerializeString(message);

                if (NodeInfo.TinctTaskInfoList.Count == 0)
                {
                    var node = NodeRepository.Current.GetNodeByName(NodeInfo.NodeName);
                    foreach (var task in node.TinctTaskInfoList)
                    {
                        task.Status = TinctTaskStatus.Faulted;
                        TinctTaskRepository.Current.UpdateTinctTasksStatus(task);
                    }
                    node.TinctTaskInfoList = null;
                }

                foreach (var tinctTaskInfo in NodeInfo.TinctTaskInfoList)
                {
                    TinctTaskRepository.Current.UpdateTinctTasksStatus(tinctTaskInfo);
                }
            }

            if (message.StartsWith("{\"ID\""))
            {
                TinctTaskRepository.Current.UpdateTinctTasksStatus(message);
            }

            return true;
        }
    }
}
