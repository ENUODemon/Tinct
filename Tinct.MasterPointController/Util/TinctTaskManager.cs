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
using Tinct.Net.Message.Node;
using Tinct.Common.Log;
using Tinct.Net.Communication.Node;
using Tinct.TinctTaskMangement.Monitor;
using Tinct.TinctTaskMangement.Util;
using Tinct.TinctTaskMangement.Handler;

namespace Tinct.TinctTaskMangement.TinctWork
{
    public class TinctTaskManeger : ITinctTaskManeger
    {

        private ITinctMangerService service;
        public TinctTaskManeger()
        {
            service = new TinctMangerService();
        }

        public ITinctTaskRepository TaskRepository { get; set; }

        public void Start()
        {

      
            TinctMasterNode.Current.StartMaster();
            TinctMasterNode.Current.MessageHandlers.Add(new MessageTaskHandler());

           // System.Threading.Thread.Sleep(20000);
           // service.RegisterRestoreMasterNode(7);
           service.RegisterCheckUnConnectNode();

            while (true)
            {
                var currenttask = TaskRepository.GetAvailableTinctTask();
                currenttask.Start();
            }

        }

        public void Stop()
        {
            TaskRepository.ClearAllTinctTasks();
        }

    }
}
