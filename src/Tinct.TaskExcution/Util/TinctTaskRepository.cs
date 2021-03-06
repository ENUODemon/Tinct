﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Common.Log;
using Newtonsoft.Json;
using System.IO;
using Tinct.Net.Message.Message;
using System.Threading;
using System.Collections.Concurrent;
using Tinct.Common.Extension;

namespace Tinct.TaskExcution.Util
{
    public class TinctTaskRepository 
    {
        public ILogger logger { get; set; }

        private ManualResetEvent syncTaskSignal = new ManualResetEvent(false);

        private ConcurrentQueue<TinctTask> queueTasks = new ConcurrentQueue<TinctTask>();

        private object syncWaittingtasks = new object();
        private List<TinctTask> queueWaittingTasks = new List<TinctTask>();

        private object syncruntimetasks = new object();
        private List<TinctTask> currentRuntimeTasks = new List<TinctTask>();

        public static TinctTaskRepository Current { get; private set; }

        static TinctTaskRepository()
        {
            Current = new TinctTaskRepository();
        }

        public TinctTaskRepository()
        {
            Current = this;
        }

        public TinctTask DequeueTinctTask()
        {
            if(queueTasks.Count==0)
            {
                if (queueWaittingTasks.Count == 0)
                {
                    syncTaskSignal.WaitOne();
                }
                else
                {
                    Thread.Sleep(5000);
                    lock (syncWaittingtasks)
                    {
                        var task = queueWaittingTasks.FirstOrDefault();
                        if (task == null)
                        {
                            syncTaskSignal.WaitOne();
                        }
                        else
                        {
                            //task.Status = TinctTaskStatus.WaittingToRun;
                            queueWaittingTasks.Remove(task);
                            QueueTinctTask(task);
                        }
                    }
                }
             
            }


          
            queueTasks.TryDequeue(out TinctTask result);
            if (queueTasks.Count == 0)
            {
                syncTaskSignal.Reset();
            }
            result.Status = TinctTaskStatus.WaittingToRun;
            logger.LogMessage(result.ToJsonSerializeString());
            return result;
        }

        public void QueueTinctTask(TinctTask task)
        {
            queueTasks.Enqueue(task);
            if(task.Status == TinctTaskStatus.Created)
            {
                logger.LogMessage(task.ToJsonSerializeString());
            }
            syncTaskSignal.Set();
        }

        public void AddWaittingTask(TinctTask task)
        {
            lock (syncWaittingtasks)
            {
                task.Status = TinctTaskStatus.Waitting;
                queueWaittingTasks.Add(task);
            }
           
            logger.LogMessage(task.ToJsonSerializeString());
        }

        public void UpdateWaittingTask(TinctTask task)
        {
            
            lock (syncWaittingtasks)
            {
                var currenttask = queueWaittingTasks.Where(t => t.ClassName == task.ClassName&&t.MethodName==task.MethodName).FirstOrDefault();
                if (currenttask == null) { return; }
                switch (task.Status)
                {
                    case TinctTaskStatus.Completed:
                        queueWaittingTasks.Remove(currenttask);
                        QueueTinctTask(currenttask);
                        
                        break;
                    case TinctTaskStatus.Exception:
                    case TinctTaskStatus.Faulted:

                        queueWaittingTasks.Remove(currenttask);
                        QueueTinctTask(currenttask);
                        break;

                    default: break;
                }

            }
        }

        public void AddRunningTask(TinctTask task)
        {
            lock (syncruntimetasks)
            {
                task.Status = TinctTaskStatus.Running;
                currentRuntimeTasks.Add(task);
            }
            logger.LogMessage(task.ToJsonSerializeString());
        }

        public void UpdateRunningTask(TinctTask task)
        {

            lock (syncruntimetasks)
            {
                var currenttask = currentRuntimeTasks.Where(t => t.ID == task.ID).FirstOrDefault();
                if (currenttask == null) { return; }
                currenttask.Status = task.Status;
                switch (task.Status)
                {
                    case TinctTaskStatus.Completed:
                        currentRuntimeTasks.Remove(currenttask);
                        if (currenttask.TinctTaskCompleted != null)
                        {
                            new Task(()=> { currenttask.TinctTaskCompleted(currenttask); }).Start();
                        }
                        logger.LogMessage(task.ToJsonSerializeString());
                        break;
                    case TinctTaskStatus.Exception:
                    case TinctTaskStatus.Faulted:

                        currentRuntimeTasks.Remove(currenttask);
                        logger.LogMessage(task.ToJsonSerializeString());
                        break;

                    default: break;
                }

            }
        }

        public void ClearAllTinctTasks()
        {
            queueTasks = new ConcurrentQueue<TinctTask>();

            lock (syncruntimetasks)
            {
                foreach (var item in currentRuntimeTasks)
                {
                    item.Status = TinctTaskStatus.Faulted;
                    item.EndTime = DateTimeExtension.GetTimeStamp();
                    logger.LogMessage(item.ToJsonSerializeString());
                }

                currentRuntimeTasks.Clear();

            }
            lock (syncWaittingtasks)
            {
                queueWaittingTasks.Clear();
            }

        }

        public List<string> GetTaskRuntimeNodeNames(TinctTask task)
        {
            List<string> result = new List<string>();
            var list = currentRuntimeTasks.Where(t => t.ClassName == task.ClassName && t.MethodName == task.MethodName).Select(t => t.TargetNodeName);
            return list == null?null:list.ToList() ;
        }



        public List<TinctTask> GetCurrentTasks()
        {
            List<TinctTask> lists = new List<TinctTask>();

            lock (syncruntimetasks)
            {
                lists.AddRange(currentRuntimeTasks.ToArray());
            }
            lock(syncWaittingtasks)
            {
                lists.AddRange(queueWaittingTasks.ToArray());
            }

            return lists;
        }
    }
}
