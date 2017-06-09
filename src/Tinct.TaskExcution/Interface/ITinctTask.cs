using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.TaskExcution.Interface
{
    public interface ITinctTask
    {
        /// <summary>
        /// Start a TinctTask
        /// </summary>
        void Start();
        /// <summary>
        /// Cancel a TinctTask
        /// </summary>
        void Cancel();
        /// <summary>
        /// Wait a TinctTask ,Infinite time
        /// </summary>
        void Wait();
        /// <summary>
        /// Wait a TinctTask,when the millsenconds pasted ,it will cancel it
        /// </summary>
        /// <param name="millsencond">waited  millsencond</param>
        void Wait(int millsencond);
        /// <summary>
        /// Dipose the TinctTasl
        /// </summary>
        void Dispose();


    }
}
