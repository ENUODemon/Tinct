using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Message.Node
{
    public enum NodeStatus
    {
        Running,
        Closed,
        BreakDown,
        Deploy,
        DeployFailed

    }
}
