
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Tinct.Net.Message.Task
{
    [Serializable]
    public class TinctTaskContext : MarshalByRefObject
    {
        public string NodeName { get; set; }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public string TaskData { get; set; }

        public string ExceptionOrFaultStrings
        {
            get;

            set;

        }


    }
}