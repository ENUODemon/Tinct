using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Node;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.Interface;

namespace Tinct.TinctTaskMangement.Util
{
    public class TinctTaskRunningState : ITintTaskState
    {
        public void HandleState(TinctWork.TinctTask task)
        {
        
            var runnode=  NodeRepository.Current.GetNodeByName(task.Context.NodeName);

            runnode.TinctTaskInfoList.Add((TinctTaskInfo)task);
        }

        public void HandleRestoreState(TinctWork.TinctTask task)
        {
            task.Command = CommandType.SyncTask;
            task.State = new TinctTaskWaitingToRunState();
            task.Status = TinctTaskStatus.WaittingToRun;


          
        }
    }
}
