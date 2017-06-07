using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tinct.Common.Extension;
using Tinct.Common.Log;
using Tinct.Net.Communication.Cfg;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Communication.Slave;
using Tinct.Net.Message.Message;
using Tinct.TinctTaskMangement.Util;

namespace Tinct.TinctTaskMangement.Handler
{
   
    public class TinctMessageSlaveHandler: MarshalByRefObject,IMessageHandler
    {

        public string LoggerName { get; set; }

        public string loggerFileName { get; set; }

        private string excuteDomainKey = "AssemblyExcuteEnvironment";

        private dynamic slaveRuntimeEngine;

        public bool HanderMessage(TinctMessage message)
        {

          switch (message.MessageHeader.CommandType)
            {
                case CommandType.Run:
                    new Task(() => { RunCommand(message.MessageBody); }).Start();
                    break;
                case CommandType.Search: break;
                case CommandType.Cancel:
                    new Task(() => { CancelCommand(message.MessageBody); }).Start();
                    break;
                case CommandType.SyncTask: SyncTaskCommand(message.MessageBody); break;

                case CommandType.Deploy: DeployCommand(message.MessageBody);
                    break;
            }

            return true;

        }

        private void DeployCommand(MessageBody messageBody)
        {
            var file1 = FileTask.GetObjectBySerializeString(messageBody.Datas);
            try
            {
                AssemblyExcuteEnvironment.Current.UnloadDomain(excuteDomainKey);
                File.WriteAllBytes(file1.FileName, file1.Content.ToArray());
            }
            catch(Exception e)
            {

            }
        }

        private void RunCommand(MessageBody messageBody)
        {

            AssemblyExcuteEnvironment.Current.AppDomainDicts.TryGetValue(excuteDomainKey, out AppDomain runtimeDomain);
            if (runtimeDomain == null)
            {
                AppDomain.CurrentDomain.SetShadowCopyFiles();
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                setup.CachePath = setup.ApplicationBase;
                setup.ShadowCopyFiles = "true";
                setup.ShadowCopyDirectories = setup.ApplicationBase;
                runtimeDomain = AppDomain.CreateDomain(excuteDomainKey, null, setup);
                AssemblyExcuteEnvironment.Current.AppDomainDicts.TryAdd(excuteDomainKey, runtimeDomain);
    
            }

            if (slaveRuntimeEngine == null)
            {
                slaveRuntimeEngine = runtimeDomain.CreateInstanceFrom
                 ("Tinct.TinctTaskMangement.dll",
                 "Tinct.TinctTaskMangement.Enigine.AssemblyTaskExcute").Unwrap();
            }
            try
            {
                
                MethodInfo method = slaveRuntimeEngine.GetType().GetMethod("ExuteTask");
                method.Invoke(slaveRuntimeEngine, new object[] { messageBody.Datas, LoggerName,loggerFileName });

            }
            catch (Exception e)
            {

            }
           
        }

        private void CancelCommand(MessageBody messageBody)
        {

            //var targeDomainkey = routData.Controller + "\\" + routData.ActionName;
            //lock (appDomainDicts)
            //{
            //    UnloadDomain(targeDomainkey);
            //}
            //Console.WriteLine("Cancel task!");
            //routData.TinctTaskInfo.Status = TinctTaskStatus.Canceled;
            //TinctSlaveNode.Current.SendMessageToMaster(routData.TinctTaskInfo.ToJsonSerializeString(), masterName);
            //TinctSlaveNode.Current.RemoveNodeTaskInfo(routData.TinctTaskInfo);
        }

        private void SyncTaskCommand(MessageBody messageBody)
        {
            //var listTasks_log= SlaveLogManger.GetSlaveTaskInfoByLog(7);
            //var returnNode = TinctSlaveNode.Current.SlaveNodeInfo.Clone();
            //returnNode.TinctTaskInfoList.AddRange(listTasks_log);
            //var syncTasks = returnNode.TinctTaskInfoList.FindAll(t => t.ID == routData.TaskID);
            //if (syncTasks != null && syncTasks.Count > 0)
            //{
            //    var syncTask = syncTasks.OrderByDescending(t => t.Status).First();
            //    TinctSlaveNode.Current.SendMessageToMaster(syncTask.ToJsonSerializeString(), masterName);
            //}
        }

    }
}
