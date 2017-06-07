using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Message.Message;

namespace Tinct.Net.Communication.Interface
{
    public interface IConnect
    {

        bool SendMessage(string machineName, int port, PackageMessage message);

        PackageMessage GetReceviceMessage();

        bool ReceviceMessage(int port);

        void CloseConnectResource();

    }
}
