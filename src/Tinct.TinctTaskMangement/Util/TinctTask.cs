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
using Tinct.TinctTaskMangement.Util;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Message;
using Tinct.Common.Extension;
using Newtonsoft.Json.Converters;

namespace Tinct.TinctTaskMangement.Util
{
    public class TinctTask : MarshalByRefObject, ITinctTask
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public bool IsCancel { get; protected set; }

        public long CreateTime { get; protected set; } = DateTimeExtension.GetTimeStamp();

        public long StartTime { get; set; }

        public long EndTime { get; set; }

        public bool HasException { get; set; }

        public string ExceptionString { get; set; }
        [JsonIgnore]
        public Exception Exption { get; set; }

        public string DllName { get; set; }

        public string NamespaceName { get; set; }

        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public string Datas { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TinctTaskStatus Status { get; set; }

        public string TargetNodeName { get; set; }

        [JsonIgnore]
        public Action<TinctTask> TinctTaskCompleted;


        //public override TinctTaskStatus Status
        //{
        //    get
        //    {
        //        return base.Status;
        //    }
        //    set
        //    {
        //        if (value != base.Status)
        //        {
        //            base.Status = value;
        //            if (value == TinctTaskStatus.Completed || value == TinctTaskStatus.Exception
        //                || value == TinctTaskStatus.Faulted || value == TinctTaskStatus.PartCompleted)
        //            {
        //                EndTime = DateTime.Now;
        //            }

        //            LogTinctTaskInfo((TinctTaskInfo)this);
        //            TinctTaskStatusChanged(value);
        //        }
        //    }
        //}

        public void Start()
        {





            //runNetTask = new Task(() =>
            //{
            //    List<NodeInfo> nodes = null;
            //    if (string.IsNullOrEmpty(Context.NodeName))
            //    {
            //        while (string.IsNullOrEmpty(Context.NodeName))
            //        {
            //            if (cancets.IsCancellationRequested)
            //            {
            //                break;
            //            }

            //            lock (TinctTask.SyncTaskRun)
            //            {
            //                nodes = NodeRepository.Current.GetAvaliableNodes();

            //                foreach (var node in nodes)
            //                {
            //                    //var findtask = node.TinctTaskInfoList.Find
            //                    //    (task => task.Context.ControllerName == Context.ControllerName
            //                    //    && task.Context.ActionName == Context.ActionName
            //                    //    && task.Status != TinctTaskStatus.Completed);
            //                    //if (findtask == null)
            //                    //{
            //                    //    this.Context.NodeName = node.NodeName;
            //                    //    StartTime = DateTime.Now;
            //                    //    Thread.Sleep(100);
            //                    //    Status = TinctTaskStatus.Running;
            //                    //    string message = ((TinctTaskInfo)this).ToJsonSerializeString();
            //                    //    TinctMasterNode.Current.SendMessageToSlave(message, this.Context.NodeName);

            //                    //    break;
            //                    //}
            //                    //else
            //                    //{

            //                    //}
            //                }

            //            }
            //            Thread.Sleep(1000);
            //        }

            //    }
            //    else
            //    {
            //        nodes = NodeRepository.Current.GetAvaliableNodes();
            //        var runnode = nodes.FirstOrDefault(n => n.NodeName == Context.NodeName);
            //        if (runnode == null)
            //        {
            //            Status = TinctTaskStatus.Faulted;
            //            return;
            //        }
            //    }


            //}, cancets.Token);

            //runNetTask.Start();
        }

        public void Cancel()
        {
            //if (Status == TinctTaskStatus.WaittingToRun)
            //{
            //    cancets.Cancel();
            //}
            //else if (Status == TinctTaskStatus.Running)
            //{
            //    this.Command = CommandType.Cancel;
            //    string message = ((TinctTaskInfo)this).ToJsonSerializeString();
            //    IsCancel = true;
            //    new Task(() =>
            //    {
            //     //   TinctMasterNode.Current.SendMessageToSlave(message, this.Context.NodeName);
            //    }).Start();
            //}
            //else if (Status == TinctTaskStatus.Completed)
            //{
            //    return;
            //}
            //Status = TinctTaskStatus.Canceled;

        }

        public void Wait()
        {
            //syncWaitObject.WaitOne();
            //if (!IsCancel)
            //{
            //    Status = TinctTaskStatus.Completed;
            //}
            //else
            //{
            //    Status = TinctTaskStatus.Canceled;
            //}

        }

        public void Wait(int millsecond)
        {
            //if (syncWaitObject.WaitOne(millsecond))
            //{
            //    Status = TinctTaskStatus.Completed;
            //}
            //else
            //{
            //    Status = TinctTaskStatus.Canceled;
            //}

        }

        public void Dispose()
        {
            //syncWaitObject.Dispose();
            //if (runNetTask.Status == TaskStatus.RanToCompletion)
            //{
            //    runNetTask.Dispose();
            //}
        }


        public static TinctTask GetObjectBySerializeString(string serializeString)
        {
            TinctTask obj = null;
            JsonSerializer serializer = new JsonSerializer();
            using (StringReader reader = new StringReader(serializeString))
            {
                try
                {
                    obj = serializer.Deserialize<TinctTask>(new JsonTextReader(reader));
                }
                catch
                {
                    throw;
                }
                return obj;
            }
        }

        public string ToJsonSerializeString()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, this);
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}
