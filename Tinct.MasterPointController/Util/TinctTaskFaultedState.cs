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
    public class TinctTaskFaultedState : ITintTaskState
    {

        public void HandleState(TinctWork.TinctTask task)
        {
            task.Context.ExceptionOrFaultStrings = "";
            task.Context.NodeName = "";
            task.StartTime = new DateTime();
            task.Status = TinctTaskStatus.WaittingToRun;
            
        }


        public void HandleRestoreState(TinctTask task)
        {
        }
    }
}
