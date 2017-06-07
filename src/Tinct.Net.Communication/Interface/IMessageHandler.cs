using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Message;

namespace Tinct.Net.Communication.Interface
{
    public interface IMessageHandler
    {
        /// <summary>
        /// true mean that deliver to next handler ,false mean that break
        /// </summary>
        /// <param name="message">hanlder  node message </param>
        /// <returns></returns>
        bool HanderMessage(TinctMessage message);
    }
}
