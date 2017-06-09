using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Interface;
using Tinct.Net.Communication.Master;
using System.Threading;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Tinct.Common.Log;
using Tinct.Net.Communication.Node;
using Tinct.TinctTaskMangement.Handler;
using Tinct.Net.Message.Message;
using Tinct.Common.Extension;
using Tinct.TaskExcution.Util;

namespace Tinct.TinctTaskMangement
{
    public class TinctTaskManeger : ITinctTaskManeger
    {

        public TinctTaskRepository TaskRepository { get; set; }

       
        public void Start()
        {
            if (TaskRepository == null)
            {
                throw new Exception("Do not find TaskRepository!");
            }

           
            new Task(() =>
            {
                while (true)
                {
                    var currenttask = TaskRepository.DequeueTinctTask();
                    var runnodes = TaskRepository.GetTaskRuntimeNodeNames(currenttask);
                    var node = GetRunningNode(TinctMasterNode.Current.SlaveNodes, runnodes);

                    if (string.IsNullOrEmpty(node))
                    {
                        TaskRepository.AddWaittingTask(currenttask);
                        continue;
                    }

                    currenttask.TargetNodeName = node;
                    currenttask.StartTime = DateTimeExtension.GetTimeStamp();
                    currenttask.Status = TinctTaskStatus.Running;
                    var message = BuildTaskMessage(currenttask, CommandType.Run);
                  
                    TinctMasterNode.Current.SendMessage(node, message);
                    TaskRepository.AddRunningTask(currenttask);
                }
            }).Start() ;


        }

        private string GetRunningNode(List<NodeInfo> allNodes, List<string> runningNodes)
        {
            var allNodesNames = allNodes.Select(t => t.NodeName);
            var nodes = allNodesNames.Where(node => !runningNodes.Contains(node)).ToList();

            int count = nodes.Count;
            if (count == 0)
            {
                return null;
            }
            Random ra = new Random();
            return nodes[ra.Next(count)];

        }

        private TinctMessage BuildTaskMessage(TinctTask tinctTask, CommandType type)
        {
            TinctMessage tmsg = new TinctMessage();
            tmsg.MessageHeader = new MessageHeader() { CommandType = type };
            tmsg.MessageBody = new MessageBody();
            tmsg.MessageBody.Datas = tinctTask.ToJsonSerializeString();
            return tmsg;
        }

        public void Stop()
        {
            TaskRepository.ClearAllTinctTasks();
        }

    }
}
