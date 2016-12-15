using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Extension;

namespace Tinct.Net.Message.Task
{
    [Serializable]
    public class TinctTaskInfo : MarshalByRefObject
    {
        [JsonIgnore]
        private List<Guid> waitTaskIDs = new List<Guid>();
        [JsonIgnore]
        private TinctTaskContext context = new TinctTaskContext();

        public Guid ID
        {
            get;
            set;
        }

        public string Name { get; set; }
        public bool IsCancel { get; protected set; }

        [JsonConverter(typeof(IsoDateTimeConverterExtension), "yyyy'-'MM'-'dd' 'HH':'mm':'ss")]
        public DateTime CreateTime { get; protected set; }
        [JsonConverter(typeof(IsoDateTimeConverterExtension), "yyyy'-'MM'-'dd' 'HH':'mm':'ss")]
        public DateTime StartTime { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverterExtension), "yyyy'-'MM'-'dd' 'HH':'mm':'ss")]
        public DateTime EndTime { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TaskPriority Priority { get; set; }

        public TinctTaskContext Context { get { return context; } set { context = value; } }

        [JsonConverter(typeof(StringEnumConverter))]
        public virtual  TinctTaskStatus Status { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public CommandType  Command{get;set;}

        [JsonIgnore]
        public List<Guid> WaitTaskIDs { get { return waitTaskIDs; } set { waitTaskIDs = value; } }


        public static TinctTaskInfo GetObjectBySerializeString(string serializeString)
        {
            TinctTaskInfo obj = null;
            JsonSerializer serializer = new JsonSerializer();
            using (StringReader reader = new StringReader(serializeString))
            {
                try
                {
                    obj = serializer.Deserialize<TinctTaskInfo>(new JsonTextReader(reader));
                }
                catch
                {
                    throw;
                }
                return obj;
            }
        }


        public  string ToJsonSerializeString()
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
