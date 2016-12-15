using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;

namespace Tinct.Net.Communication.Interface
{
    public interface ITinctConnect
    {
        /// <summary>
        /// when recvice message from node ,will raise the event.
        /// </summary>
        event EventHandler<ReceiveMessageArgs> MessageHandlers;

        /// <summary>
        /// when client close connection,will raise the event.
        /// </summary>
        event EventHandler<ReceiveMessageArgs> MachineCloseConnectHandlers;
        /// <summary>
        /// Listen port for connect 
        /// </summary>
        /// <param name="port">int param network param</param>
        /// <returns>true for sucessfullu listen ,false for failed</returns>
        /// <exception></exception>
        bool ListenningPort(int port);

        /// <summary>
        /// stop listening
        /// </summary>
        /// <returns>true for successfully ,fasle for stop failed</returns>
        /// <exception></exception>
        bool StopListenning();


        /// <summary>
        /// send message to target machine
        /// </summary>
        /// <param name="machineName">the target machine name</param>
        /// <param name="port">the target machine port</param>
        /// <param name="message">message want to be sended</param>
        /// <returns>true declare can be connect to target machine,false declare can not be to target machine</returns>
        bool SendMessage(string machineName, int port, string message);




    }
}
