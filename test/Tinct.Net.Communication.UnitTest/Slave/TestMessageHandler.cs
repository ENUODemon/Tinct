using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Message;

namespace Tinct.Net.Communication.UnitTest.Slave
{
    public class TestMessageHandler : IMessageHandler
    {

        public string Message = "";

        public bool HanderMessage(TinctMessage message)
        {
            Message = message.MessageBody.Datas;
            return true;
        }
    }
}
