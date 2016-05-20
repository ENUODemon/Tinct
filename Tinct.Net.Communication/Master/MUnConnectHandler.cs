using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Communication.Node;

namespace Tinct.Net.Communication.Master
{
    public class MUnConnectHandler : IUnConnectNodeHandler
    {

        public void HandleUnConnectNode(Message.Node.NodeInfo node)
        {
            NodeRepository.Current.RemoveNode(node);
        }
    }
}
