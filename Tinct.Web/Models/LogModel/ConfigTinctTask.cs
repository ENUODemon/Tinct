using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.Web.Models.LogModel
{
    public class ConfigTinctTask
    {
        public string Name { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string TaskData { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TaskPriority Priority { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TinctTaskStatus TaskStatus { get; set; }


        //public TinctTask ConvertToTinctTask()
        //{
        //    TinctTask task = new TinctTask();
        //    task.Name = Name;
        //   // task.Context.TaskID = task.ID;
        //    task.Context.ControllerName = ControllerName;
        //    task.Context.ActionName = ActionName;
        //    task.Priority = Priority;
        //  //  task.TaskStatus = TaskStatus;
        //    task.Context.TaskData = TaskData;


        //    return task;
        //}

    }
}