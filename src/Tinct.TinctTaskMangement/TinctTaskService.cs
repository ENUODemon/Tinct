using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Common.Log;
using Tinct.Net.Communication.Cfg;
using Tinct.Net.Communication.Master;
using Tinct.Net.Communication.Slave;
using Tinct.Net.Message.Message;
using Tinct.TinctTaskMangement.Handler;
using Tinct.TinctTaskMangement.Util;

namespace Tinct.TinctTaskMangement
{
    public class TinctTaskService
    {
       
        public static void StartMasterService()
        {
           
           
            TinctMasterNode.Current.MessageHandlers.Add(new TinctMessageMasterHandler());
            TinctMasterNode.Current.StartMaster();
          

        }

        public static void StartSlaveService(string loggerName, string loggerFileName)
        {
          
            TinctMessage msg = new TinctMessage();
            msg.MessageBody = new MessageBody();
            msg.MessageBody.Datas = "connect";
            var slavehandler = new TinctMessageSlaveHandler();
            slavehandler.LoggerName = loggerName;
            slavehandler.loggerFileName = loggerFileName;
            TinctSlaveNode.Current.MessageHandlers.Add(slavehandler);
            TinctSlaveNode.Current.StartSlave();
            TinctSlaveNode.Current.SendMessage(TinctNodeCongratulations.MasterName, msg);

        }

        public static void StartTaskService(TinctTaskRepository repository, string loggerName, string loggerFileName)
        {
            var logger = TinctLoggerManger.GetLogger(loggerName, loggerFileName);
            repository.logger = logger;
            TinctTaskMangement.TinctTaskManeger tm = new TinctTaskMangement.TinctTaskManeger();

            tm.TaskRepository = repository;
            tm.Start();
        }

        public static void DeployFile(FileTask file)
        {
            var bytes= File.ReadAllBytes(file.SourcePath);
            file.Content.AddRange(bytes);
            TinctMessage msg = new TinctMessage();
            msg.MessageHeader = new MessageHeader();
            msg.MessageHeader.CommandType = CommandType.Deploy;
            msg.MessageBody = new MessageBody();
            msg.MessageBody.Datas = file.ToJsonSerializeString();
            TinctMasterNode.Current.SendMessage(TinctNodeCongratulations.MasterName, msg);
        }

    }
}
