using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Interface;
using Tinct.Net.Message.Node;
using Tinct.Common.Log;
using Newtonsoft.Json;
using System.IO;
using Tinct.TinctTaskMangement.Monitor;
using Tinct.Net.Message.Task;
using Tinct.Net.Message.Message;
using System.Threading;


namespace Tinct.TinctTaskMangement.TinctWork
{
    public class TinctTaskRepository : ITinctTaskRepository
    {
        private ILogger logger;
        private List<TinctTask> tinctTasks;
        private List<TinctTask> runtimeTasks;
        private object syncqueue;
        private object synctasks;
        private ManualResetEvent syncTaskSignal = new ManualResetEvent(false);

        public static TinctTaskRepository Current { get; private set; }
        static TinctTaskRepository()
        {
            Current = new TinctTaskRepository();
        }

        public TinctTaskRepository()
        {

            #region initialize
            logger = TinctLogManager.Current.GetMLogger();
            tinctTasks = new List<TinctTask>();
            runtimeTasks = new List<TinctTask>();
            syncqueue = new object();
            synctasks = new object();
            #endregion


            Current = this;
        }

        public IEnumerable<TinctTask> GetCurrentTinctTasks()
        {
            return tinctTasks;
        }

        public TinctTask GetAvailableTinctTask()
        {

            syncTaskSignal.WaitOne();
            lock (syncqueue)
            {
                try
                {
                    var task = runtimeTasks.First();
                    runtimeTasks.Remove(task);


                    if (runtimeTasks.Count == 0)
                    {
                        syncTaskSignal.Reset();
                    }

                    return task;
                }
                catch
                {
                    return null;
                }
            }

        }

        public void AddTinctTask(TinctTask task)
        {

            lock (synctasks)
            {
                if (tinctTasks.Count!=0&&tinctTasks.SingleOrDefault(x => x.ID == task.ID) != null)
                {

                }
                else
                {
                    tinctTasks.Add(task);
                }
            }
            if (task.Status == TinctTaskStatus.WaittingToRun)
            {
                lock (syncqueue)
                {

                    switch (task.Priority)
                    {
                        case TaskPriority.Medium:
                            var mediumIndex = runtimeTasks.FindIndex(item => item.Priority == TaskPriority.Low);
                            if (mediumIndex == -1)
                            {
                                runtimeTasks.Add(task); break;
                            }
                            runtimeTasks.Insert(mediumIndex, task); break;
                        case TaskPriority.High:
                            var highIndex = runtimeTasks.FindIndex(item => item.Priority == TaskPriority.Medium);
                            if (highIndex == -1)
                            {
                                var sechighIndex = runtimeTasks.FindIndex(item => item.Priority == TaskPriority.Low);
                                if (sechighIndex == -1)
                                {
                                    runtimeTasks.Add(task); break;
                                }
                                runtimeTasks.Insert(sechighIndex, task); break;
                            }
                            runtimeTasks.Insert(highIndex, task); break;
                        case TaskPriority.Low:
                            runtimeTasks.Add(task); break;
                    }
                    syncTaskSignal.Set();
                }
            }
        }

        public TinctTask GetTinctTaskByID(Guid ID)
        {
            lock (synctasks)
            {
                if (tinctTasks.Count != 0)
                {
                    return tinctTasks.SingleOrDefault(x => x.ID == ID);
                }
                else 
                {
                    return null;
                }
            }
        }

        public bool RemoveTinctTask(TinctTask task)
        {
            lock (synctasks)
            {
                return tinctTasks.Remove(task);
            }
        }

        public void UpdateTinctTasksStatus(string message)
        {
            var tinctTaskInfo = TinctTaskInfo.GetObjectBySerializeString(message);

            lock (synctasks)
            {
                var tinctTask = GetTinctTaskByID(tinctTaskInfo.ID);
                tinctTask.Context = tinctTaskInfo.Context;
                if (tinctTaskInfo.Status == TinctTaskStatus.Canceled) 
                {
                    tinctTask.EndTime = DateTime.Now;
                }
                tinctTask.Status = tinctTaskInfo.Status;
                
            }
        }

        public void UpdateTinctTasksStatus(TinctTaskInfo taskInfo)
        {

            lock (synctasks)
            {
                var tinctTask = GetTinctTaskByID(taskInfo.ID);
                tinctTask.Status = taskInfo.Status;
            }
        }



        public void ClearAllTinctTasks()
        {
            runtimeTasks.Clear();
            tinctTasks.Clear();
          
        }
    }
}
