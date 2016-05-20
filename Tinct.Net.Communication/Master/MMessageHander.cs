using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Communication.Node;

namespace Tinct.Net.Communication.Master
{
    public class MMessageHander:IMessageHandler
    {

        public bool HanderMessage(string message)
        {
            if (message.StartsWith("{\"NodeName\""))
            {
                NodeRepository.Current.UpdateNodeInfo(message);
            }

            return true;
        }
    }
}
