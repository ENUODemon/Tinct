using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Cfg;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Communication.Slave;
using Tinct.Net.Message.Message;
using Tinct.TinctTaskMangement.Util;

namespace Tinct.TinctTaskMangement.Handler
{
    public class TinctMessageMasterHandler : IMessageHandler
    {
        public bool HanderMessage(TinctMessage message)
        {
           
            switch (message.MessageHeader.CommandType)
            {
                case CommandType.Return:
                   // TinctSlaveNode.Current.AddNodeTaskInfo(routData.TinctTaskInfo);
                    new Task(() => { ReturnCommand(message.MessageBody); }).Start();
                    break;
               
            }

            return true;

        }

        private void ReturnCommand(MessageBody messageBody)
        {
            var task = TinctTask.GetObjectBySerializeString(messageBody.Datas);
            TinctTaskRepository.Current.UpdateRunningTask(task);
            TinctTaskRepository.Current.UpdateWaittingTask(task);
        }



      
    }
}

