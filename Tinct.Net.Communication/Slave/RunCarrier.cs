using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Communication.Slave
{
    public  class RunCarrier
    {
        private Dictionary<string, AppDomain> appDomainDicts;

        public Dictionary<string, AppDomain> AppDomainDicts { get{return appDomainDicts;}  }


        public static RunCarrier Current { get; private set; }
        static RunCarrier() 
        {
            Current = new RunCarrier();
        }

        public RunCarrier() 
        {
            appDomainDicts = new Dictionary<string, AppDomain>();
        }

       

    }
}
