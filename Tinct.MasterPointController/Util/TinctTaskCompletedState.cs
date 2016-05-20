using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Node;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.Interface;

namespace Tinct.TinctTaskMangement.Util
{
    public class TinctTaskCompletedState: ITintTaskState
    {

        public void HandleState(TinctWork.TinctTask task)
        {
            var runnode = NodeRepository.Current.GetNodeByName(task.Context.NodeName);
            var taskinfo = runnode.TinctTaskInfoList.FirstOrDefault(t => t.ID == task.ID);
    
            runnode.TinctTaskInfoList.Remove(taskinfo);
        }

        public void HandleRestoreState(TinctWork.TinctTask task)
        {
            throw new NotImplementedException();
        }
    }
}
