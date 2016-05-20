using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Util
{
    public class TinctTaskWaitingState : ITintTaskState
    {
        public void HandleState(TinctTask task)
        {
            new Task(() =>
            {
                foreach (var waitTask in task.WaittingTinctTasks)
                {
                    waitTask.Wait();
                }
                task.Status = TinctTaskStatus.WaittingToRun;
            }).Start(); ;
            task.State = new TinctTaskWaitingToRunState();
           
        }


        public void HandleRestoreState(TinctTask task)
        {
            HandleState(task);
        }
    }
}
