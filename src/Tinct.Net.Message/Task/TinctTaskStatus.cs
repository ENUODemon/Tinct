using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Message.Task
{
    public enum TinctTaskStatus
    {
        Created,
        WaittingToRun,
        Waitting,
        Running,
        Completed,
        PartCompleted,
        Canceled,
        Faulted,
        Exception

    }
}
