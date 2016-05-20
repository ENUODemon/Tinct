using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using System.Net.Sockets;
using System.Net;
using Tinct.Net.Communication.Connect;
using System.Configuration;

namespace Tinct.Net.Communication.Master
{
    public  abstract class MasterNode : IMasterConnect
    {
        protected ITinctConnect tinctCon = null;

        public abstract bool StartMaster();

        public abstract bool EndMaster();
       
    }
}
