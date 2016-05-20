using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Communication.Connect
{
   public  class SendMessageArgs
    {
       public string Message { get; set; }
       public TcpClient tcpClient {get;set;}
    }
}
