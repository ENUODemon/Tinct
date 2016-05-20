using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Interface
{
    public interface ITintTaskState
    {
        /// <summary>
        /// Handle State 
        /// </summary>
        /// <param name="task">the goal tasl</param>
        void HandleState(TinctTask task);

        /// <summary>
        /// Handle Restore State 
        /// </summary>
        /// <param name="task">the goal tasl</param>

        void HandleRestoreState(TinctTask task);
    }
}
