using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Communication.Interface
{
    public interface IMasterConnect
    {
        /// <summary>
        /// /// <summary>
        /// Start Slave 
        /// </summary>
        /// <returns>true for sucessfully,false for failed</returns>

        bool StartMaster();
        /// <summary>
        /// Start Slave 
        /// </summary>
        /// <returns>true for sucessfully,false for failed</returns>
        bool EndMaster();
    }
}
