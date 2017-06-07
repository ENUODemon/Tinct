using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.TinctTaskMangement.Interface
{
    public interface ITinctTaskManeger
    {
        /// <summary>
        /// Start task manager
        /// </summary>
        void Start();

        /// <summary>
        /// Stop task manger
        /// </summary>
        void Stop();

    }

}
