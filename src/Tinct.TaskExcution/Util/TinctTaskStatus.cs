﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.TaskExcution.Util
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
