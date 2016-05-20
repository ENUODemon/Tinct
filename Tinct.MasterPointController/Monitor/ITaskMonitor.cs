using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Monitor
{
    interface ITaskMonitor
    {
         List<TinctTask> GetTasksInfo();
    }
}
