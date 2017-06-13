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
using Tinct.TaskExcution.Util;
using Tinct.TinctTaskMangement.Handler;

namespace Tinct.TinctTaskMangement
{
    public static class TinctTaskService
    {

        private static bool masterServiceIsOn = false;

        private static bool taskServiceIsOn = false;

        private static bool slaveServiceIsOn = false;

        public static void StartMasterService()
        {
            if (!masterServiceIsOn)
            {
                TinctMasterNode.Current.MessageHandlers.Add(new TinctMessageMasterHandler());
                TinctMasterNode.Current.StartMaster();
                masterServiceIsOn = true;
            }
        }

        public static void StartSlaveService(string loggerName, string loggerFileName)
        {
            if (!slaveServiceIsOn)
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
                slaveServiceIsOn = true;
            }
        }

        public static void StartTaskService(TinctTaskRepository repository, string loggerName, string loggerFileName)
        {
            if (!taskServiceIsOn)
            {
                var logger = TinctLoggerManger.GetLogger(loggerName, loggerFileName);
                repository.logger = logger;
                TinctTaskMangement.TinctTaskManeger tm = new TinctTaskMangement.TinctTaskManeger();
                tm.TaskRepository = repository;
                tm.Start();
                taskServiceIsOn = true;
            }
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
            TinctTaskRepository.Current.ClearAllTinctTasks();
            for (int i = 0; i < TinctMasterNode.Current.SlaveNodes.Count; i++)
            {
                try
                {
                    TinctMasterNode.Current.SendMessage(TinctMasterNode.Current.SlaveNodes[i].NodeName, msg);
                }
                catch 
                {
                    ///when change salve node status,ingore that
                }
            }

           
        }

        public static List<TinctTask> GetCurrentTasks()
        {
            return TinctTaskRepository.Current.GetCurrentTasks();
        }
    }
}
