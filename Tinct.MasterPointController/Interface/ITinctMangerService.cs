using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.TinctTaskMangement.Interface
{
    public  interface ITinctMangerService
    {
        /// <summary>
        /// Register check UnConect node sercice
        /// </summary>
        void RegisterCheckUnConnectNode();
       /// <summary>
        /// Register Restore MasterNode when the MasterNode is down
       /// </summary>
       /// <param name="step">the long days step</param>
        void RegisterRestoreMasterNode(int step);
    }
}
