using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tinct.Net.Message.Task;

namespace Tinct.Net.Message.Node
{
    public class NodeInfo
    {
        private List<TinctTaskInfo> tinctTaskInfoList = new List<TinctTaskInfo>();
        public string NodeName { get; set; }

        public DateTime LastUpdateTime { get; set; }


        [JsonConverter(typeof(StringEnumConverter))]
        public NodeStatus Status { get; set; }


        public List<TinctTaskInfo> TinctTaskInfoList
        {
            get
            {
                return tinctTaskInfoList;
            }
            set
            {
                tinctTaskInfoList = value;
            }
        }

        public string ToJsonSerializeString()
        {
            JsonSerializer serializer = new JsonSerializer();
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);


            return sw.GetStringBuilder().ToString();
        }

        public static NodeInfo GetObjectBySerializeString(string serializeString)
        {
            NodeInfo obj = null;
            JsonSerializer serializer = new JsonSerializer();
            StringReader reader = new StringReader(serializeString);
            try
            {
                obj = serializer.Deserialize<NodeInfo>(new JsonTextReader(reader));
            }
            catch
            {
                throw;
            }
            return obj;
        }

        public NodeInfo Clone()
        {

            var node = (NodeInfo)this.MemberwiseClone();
            node.TinctTaskInfoList = new List<TinctTaskInfo>();
            foreach (var taskinfo in TinctTaskInfoList)
            {
                node.TinctTaskInfoList.Add(taskinfo);
            }

            return node;
        }
    }
}
