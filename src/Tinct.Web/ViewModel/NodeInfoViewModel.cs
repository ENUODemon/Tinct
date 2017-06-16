using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinct.Net.Communication.Node;

namespace Tinct.Web.ViewModel
{
    public class NodeInfoViewModel
    {
        public string NodeName { get; set; }

        public DateTime LastUpdateTime
        {
            get;
            set;

        }

        [JsonConverter(typeof(StringEnumConverter))]
        public NodeStatus Status { get; set; }
    }
}
