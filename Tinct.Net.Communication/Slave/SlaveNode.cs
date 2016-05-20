using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;

namespace Tinct.Net.Communication.Slave
{
    public  abstract class SlaveNode:ISlaveConnect
    {
        protected ITinctConnect tinctCon=null;
        public abstract bool StartSlave();

        public abstract bool EndSlave();

    }
}
