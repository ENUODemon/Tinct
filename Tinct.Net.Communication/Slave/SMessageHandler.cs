using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Task;
using Tinct.Net.MessageDispath.Route;
using Tinct.TinctTaskMangement.Util;

namespace Tinct.Net.Communication.Slave
{
    public class SMessageHandler : IMessageHandler
    {
        private Route route = new Route();

        private Dictionary<string, AppDomain> appDomainDicts = RunCarrier.Current.AppDomainDicts;
        private string masterName = ConfigurationManager.AppSettings["Master"].ToString();
        public bool HanderMessage(string message)
        {

       



            var routData = route.GetRouteData(message);

       


            switch (routData.CommandType)
            {
                case CommandType.Run:
                    TinctSlaveNode.Current.AddNodeTaskInfo(routData.TinctTaskInfo);
                    new Task(() => { RunCommand(routData); }).Start();


                    break;
                case CommandType.Search: break;
                case CommandType.Cancel:
                    new Task(() => { CancelCommand(routData); }).Start();

                    break;
                case CommandType.SyncTask: SyncTaskCommand(routData); break;

            }

            return true;

        }


        private void RunCommand(RouteData routData)
        {
            var runTimeActionDomain = AppDomain.CreateDomain(routData.Controller + "\\" + routData.ActionName);
            lock (appDomainDicts)
            {
                appDomainDicts.Add(runTimeActionDomain.FriendlyName, runTimeActionDomain);
            }

            var domainroute = (Route)runTimeActionDomain.CreateInstanceFromAndUnwrap("Tinct.MessageDispath.dll", "Tinct.Net.MessageDispath.Route.Route");
            try
            {
                routData = domainroute.RouteHandler.MapToControllerExcute(routData);

            }
            catch (AppDomainUnloadedException e) 
            {
                return;
            }
            catch (Exception e)
            {
                routData.TinctTaskInfo.Status = TinctTaskStatus.Exception;
                routData.TinctTaskInfo.Context.ExceptionOrFaultStrings += "[[[" + "excute has ecception" + "\r\n" + e.Message + "]]]";
            }
           

            UnloadDomain(runTimeActionDomain.FriendlyName);
           
            TinctSlaveNode.Current.SendMessageToMaster(routData.TinctTaskInfo.ToJsonSerializeString(), masterName);
        
            TinctSlaveNode.Current.RemoveNodeTaskInfo(routData.TinctTaskInfo);
         

        }

        private void CancelCommand(RouteData routData)
        {

            var targeDomainkey = routData.Controller + "\\" + routData.ActionName;
            lock (appDomainDicts)
            {
                UnloadDomain(targeDomainkey);
            }
            Console.WriteLine("Cancel task!");
            routData.TinctTaskInfo.Status = TinctTaskStatus.Canceled;
            TinctSlaveNode.Current.SendMessageToMaster(routData.TinctTaskInfo.ToJsonSerializeString(), masterName);
            TinctSlaveNode.Current.RemoveNodeTaskInfo(routData.TinctTaskInfo);
        }

        private void SyncTaskCommand(RouteData routData) 
        {
             var listTasks_log= SlaveLogManger.GetSlaveTaskInfoByLog(7);
             var returnNode = TinctSlaveNode.Current.SlaveNodeInfo.Clone();
             returnNode.TinctTaskInfoList.AddRange(listTasks_log);
             var syncTasks = returnNode.TinctTaskInfoList.FindAll(t => t.ID == routData.TaskID);
             if (syncTasks != null && syncTasks.Count > 0)
             {
                 var syncTask = syncTasks.OrderByDescending(t => t.Status).First();
                 TinctSlaveNode.Current.SendMessageToMaster(syncTask.ToJsonSerializeString(), masterName);
             }
        }

        private void UnloadDomain(string key)
        {
            AppDomain targeDomain = null;
            bool successful = false;
            lock (appDomainDicts)
            {
                successful = appDomainDicts.TryGetValue(key, out targeDomain);
                if (successful)
                {
                    appDomainDicts.Remove(targeDomain.FriendlyName);
                }
            }
            if (targeDomain != null)
            {
                AppDomain.Unload(targeDomain);
            }
        }
    }
}

