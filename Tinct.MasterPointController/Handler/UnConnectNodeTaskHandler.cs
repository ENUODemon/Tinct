using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Handler
{
    public class UnConnectNodeTaskHandler : IUnConnectNodeHandler
    {


        public void HandleUnConnectNode(Net.Message.Node.NodeInfo node)
        {
            node.TinctTaskInfoList.ForEach(
                item =>
                {
                    var task = TinctTaskRepository.Current.GetTinctTaskByID(item.ID);
                    if (task.Status == TinctTaskStatus.Running)
                    {
                        task.Context.ExceptionOrFaultStrings = "[[[" + "UnConnect to the Running Node!" + "]]]";
    
                        task.Status = TinctTaskStatus.Faulted;
                    }
                });

        }
    }
}
