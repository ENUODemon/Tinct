using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Interface;
using Tinct.Net.Communication.Master;
using Tinct.Net.Communication.Connect;
using Tinct.Common.Log;
using Tinct.Net.Communication.Node;
using Newtonsoft.Json;
using System.IO;
using Tinct.Net.Message.Node;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.Util;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Message;


namespace Tinct.TinctTaskMangement.TinctWork
{
    public class TinctTask : TinctTaskInfo, ITinctTask
    {
        private static object SyncTaskRun = new object();

        private ManualResetEvent syncWaitObject = new ManualResetEvent(false);
        private ManualResetEvent syncTaskObject = new ManualResetEvent(false);

        private Task runNetTask;

        private CancellationTokenSource cancets = new CancellationTokenSource();

        private List<TinctTask> waittingTinctTasks;
        [JsonIgnore]
        public List<TinctTask> WaittingTinctTasks
        {
            get
            {
                return waittingTinctTasks;
            }
            set
            {
                waittingTinctTasks = value;
            }
        }

        internal ITintTaskState State { get; set; }

        public event EventHandler<TinctTaskEventArgs> TinctTaskCompleted;

        #region Constructor



        public TinctTask()
        {
            ID = Guid.NewGuid();
            Name = (DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day) + "_Task";
            Context = new TinctTaskContext();
            CreateTime = DateTime.Now;
        }
        public TinctTask(String name)
        {
            ID = Guid.NewGuid();
            Name = name;
            Context = new TinctTaskContext();
            CreateTime = DateTime.Now;
        }
        public TinctTask(string name, TinctTaskContext context)
        {
            ID = Guid.NewGuid();
            Name = name;
            Context = context;
            CreateTime = DateTime.Now;
        }

        public TinctTask(TinctTaskInfo taskInfo)
        {
            base.Command = taskInfo.Command;
            base.Context = taskInfo.Context;
            base.CreateTime = taskInfo.CreateTime;
            base.EndTime = taskInfo.EndTime;
            base.ID = taskInfo.ID;
            base.IsCancel = taskInfo.IsCancel;
            base.Name = taskInfo.Name;
            base.Priority = taskInfo.Priority;
            base.StartTime = taskInfo.StartTime;
            base.Status = taskInfo.Status;
            base.WaitTaskIDs = taskInfo.WaitTaskIDs;
        }

        public override TinctTaskStatus Status
        {
            get
            {
                return base.Status;
            }
            set
            {
                if (value != base.Status)
                {
                    base.Status = value;
                    if (value == TinctTaskStatus.Completed || value == TinctTaskStatus.Exception
                        || value == TinctTaskStatus.Faulted || value == TinctTaskStatus.PartCompleted)
                    {
                        EndTime = DateTime.Now;
                    }

                    LogTinctTaskInfo((TinctTaskInfo)this);
                    TinctTaskStatusChanged(value);
                }
            }
        }
        #endregion

        public void Start()
        {
            runNetTask = new Task(() =>
            {
                List<NodeInfo> nodes = null;
                if (string.IsNullOrEmpty(Context.NodeName))
                {
                    while (string.IsNullOrEmpty(Context.NodeName))
                    {
                        if (cancets.IsCancellationRequested)
                        {
                            break;
                        }

                        lock (TinctTask.SyncTaskRun)
                        {
                            nodes = NodeRepository.Current.GetAvaliableNodes();

                            foreach (var node in nodes)
                            {
                                var findtask = node.TinctTaskInfoList.Find
                                    (task => task.Context.ControllerName == Context.ControllerName
                                    && task.Context.ActionName == Context.ActionName
                                    && task.Status != TinctTaskStatus.Completed);
                                if (findtask == null)
                                {
                                    this.Context.NodeName = node.NodeName;
                                    StartTime = DateTime.Now;
                                    Thread.Sleep(100);
                                    Status = TinctTaskStatus.Running;
                                    string message = ((TinctTaskInfo)this).ToJsonSerializeString();
                                    TinctMasterNode.Current.SendMessageToSlave(message, this.Context.NodeName);

                                    break;
                                }
                                else
                                {

                                }
                            }

                        }
                        Thread.Sleep(1000);
                    }

                }
                else
                {
                    nodes = NodeRepository.Current.GetAvaliableNodes();
                    var runnode = nodes.FirstOrDefault(n => n.NodeName == Context.NodeName);
                    if (runnode == null)
                    {
                        Status = TinctTaskStatus.Faulted;
                        return;
                    }
                }


            }, cancets.Token);

            runNetTask.Start();
        }

        public void Cancel()
        {
            if (Status == TinctTaskStatus.WaittingToRun)
            {
                cancets.Cancel();
            }
            else if (Status == TinctTaskStatus.Running)
            {
                this.Command = CommandType.Cancel;
                string message = ((TinctTaskInfo)this).ToJsonSerializeString();
                IsCancel = true;
                new Task(() =>
                {
                    TinctMasterNode.Current.SendMessageToSlave(message, this.Context.NodeName);
                }).Start();
            }
            else if (Status == TinctTaskStatus.Completed)
            {
                return;
            }
            Status = TinctTaskStatus.Canceled;

        }

        public void Wait()
        {
            syncWaitObject.WaitOne();
            if (!IsCancel)
            {
                Status = TinctTaskStatus.Completed;
            }
            else
            {
                Status = TinctTaskStatus.Canceled;
            }

        }

        public void Wait(int millsecond)
        {
            if (syncWaitObject.WaitOne(millsecond))
            {
                Status = TinctTaskStatus.Completed;
            }
            else
            {
                Status = TinctTaskStatus.Canceled;
            }

        }

        public void Dispose()
        {
            syncWaitObject.Dispose();
            if (runNetTask.Status == TaskStatus.RanToCompletion)
            {
                runNetTask.Dispose();
            }
        }

        private void LogTinctTaskInfo(TinctTaskInfo taskInfo)
        {
            ILogger logger = TinctLogManager.Current.GetMLogger();
            LogEntity entity = new LogEntity();
            entity.Message = base.ToJsonSerializeString();
            logger.LogInfo(entity);
        }

        private void TinctTaskStatusChanged(TinctTaskStatus status)
        {
            try
            {
                switch (status)
                {
                    case TinctTaskStatus.Running:

                        State = new TinctTaskRunningState();
                        State.HandleState(this);
                        break;
                    case TinctTaskStatus.PartCompleted:
                    case TinctTaskStatus.Completed:
                        State = new TinctTaskCompletedState();
                        State.HandleState(this);
                        syncWaitObject.Set();
                        if (TinctTaskCompleted != null)
                        {
                            TinctTaskCompleted(this, new TinctTaskEventArgs() { Context = this.Context });
                        }
                        break;
                    case TinctTaskStatus.Waitting:
                        State = new TinctTaskWaitingState();
                        State.HandleState(this); break;
                    case TinctTaskStatus.WaittingToRun:
                        State = new TinctTaskWaitingToRunState();
                        State.HandleState(this); break;
                    case TinctTaskStatus.Faulted:
                        State = new TinctTaskFaultedState();
                        State.HandleState(this); break;
                    case TinctTaskStatus.Canceled:

                        syncWaitObject.Set();
                        break;
                    default: break;
                }
            }
            catch
            {
            }
        }

    }
}
