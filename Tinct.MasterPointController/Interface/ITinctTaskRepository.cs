using System;
using System.Collections.Generic;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Interface
{
    public interface ITinctTaskRepository
    {
       /// <summary>
       /// Get a avaliable TinctTask
       /// </summary>
       /// <returns></returns>

        TinctTask GetAvailableTinctTask();

        /// <summary>
        /// Get Current TinctTask List
        /// </summary>
        /// <returns></returns>
        IEnumerable<TinctTask> GetCurrentTinctTasks();
        /// <summary>
        /// add task
        /// </summary>
        /// <param name="task">goal task</param>
        void AddTinctTask(TinctTask task);
        /// <summary>
        /// remove tasl
        /// </summary>
        /// <param name="task">goal task</param>
        /// <returns></returns>
        bool RemoveTinctTask(TinctTask task);
        /// <summary>
        /// update tasl
        /// </summary>
        /// <param name="message">message</param>
        void UpdateTinctTasksStatus(string message);
        /// <summary>
        /// get task by ID
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        TinctTask GetTinctTaskByID(Guid ID);

        /// <summary>
        /// clear all Tincttasks
        /// 
        /// </summary>
        void ClearAllTinctTasks();

    }
}
