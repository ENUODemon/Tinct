using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Node;

namespace Tinct.TinctTaskMangement.Monitor
{
    interface INodeMonitor
    {
         List<NodeInfo> GetNodesInfo();
    }
}
