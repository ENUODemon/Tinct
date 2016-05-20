using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Communication.Interface
{
    public  interface ISlaveConnect
    {
        /// <summary>
        /// Start Slave 
        /// </summary>
        /// <returns>true for sucessfully,false for failed</returns>
         bool StartSlave();
        /// <summary>
        /// End Slave
        /// </summary>
         /// <returns>true for sucessfully,false for failed</returns>
         bool EndSlave();
    }
}
