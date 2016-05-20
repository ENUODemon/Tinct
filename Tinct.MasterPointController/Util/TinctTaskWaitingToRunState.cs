using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Message;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Util
{
    public class TinctTaskWaitingToRunState : ITintTaskState
    {

        public void HandleState(TinctWork.TinctTask task)
        {
            TinctTaskRepository.Current.AddTinctTask(task);
        }


        public void HandleRestoreState(TinctTask task)
        {
            task.Command = CommandType.Run;
            TinctTaskRepository.Current.AddTinctTask(task);
        }
    }
}
