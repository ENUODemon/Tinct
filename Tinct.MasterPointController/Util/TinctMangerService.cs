using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Common.Log;
using Tinct.Net.Communication.Master;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.Handler;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Util
{
    public class TinctMangerService : ITinctMangerService
    {

        private ITintTaskState state;

        public void RegisterCheckUnConnectNode()
        {
            TinctMasterNode.Current.UnConnectHandlers.Add(new UnConnectNodeTaskHandler());
            TinctMasterNode.Current.StartCheckNodeService();
   
        }

        public void RegisterRestoreMasterNode(int step)
        {
            string sLoggerName = ConfigurationManager.AppSettings["MasterLoggerName"].ToString();
            var logger = TinctLogManager.Current.GetMLogger();
            List<TinctTaskInfo> listTasks = new List<TinctTaskInfo>();
            string path = logger.GetLoggerPath(sLoggerName);
            string fileName = path + "\\" + "task";
            for (int i = step; i >= 0; i--)
            {
                string desfileName = fileName + (DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + (DateTime.Now.Day - i)) + ".json";
                if (File.Exists(desfileName))
                {
                    using (StreamReader reader = new StreamReader(File.OpenRead(desfileName)))
                    {
                        while (true)
                        {
                            string infostr = reader.ReadLine();
                            if (string.IsNullOrEmpty(infostr))
                            {
                                break;
                            }
                            var entity = LogEntity.GetObjectBySerializeString(infostr);
                            var taskinfo = TinctTaskInfo.GetObjectBySerializeString(entity.Message.ToString());
                            listTasks.Add(taskinfo);
                        }
                    }
                }
                
            }
            

            listTasks = listTasks.Distinct().ToList();

            var grouptasks = listTasks.GroupBy(t => t.ID);
            foreach (var group in grouptasks) 
            {
                var lastitem = group.Last();
                var status = lastitem.Status;
                TinctTask lastTinctTask = new TinctTask(lastitem);
                var state = lastTinctTask.State;

                switch (status)
                {
                    case TinctTaskStatus.Running:
                        state = new TinctTaskRunningState();
                        state.HandleRestoreState(lastTinctTask); break;
                    case TinctTaskStatus.PartCompleted:
                    case TinctTaskStatus.Completed:  
                        break;
                    case TinctTaskStatus.Waitting: state = new TinctTaskWaitingState();
                        state.HandleRestoreState(lastTinctTask); break;
                    case TinctTaskStatus.WaittingToRun: state = new TinctTaskWaitingToRunState();
                        state.HandleRestoreState(lastTinctTask); break;
                    case TinctTaskStatus.Faulted: 
                      break;
                    case TinctTaskStatus.Canceled:
                        break;
                    default: break;
                }
               
            }
        }
    }
}
