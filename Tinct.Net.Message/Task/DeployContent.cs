using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Tinct.TinctTaskMangement.Util
{
    public class DeployContent
    {

        public string FilePath { get; set; }
        public string FileName { get; set; }


        public byte[] Content { get;  set; }

        public string TargetNodeName { get; set; }

        public string Status { get; set; }


        public string ToJsonSerializeString()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, this);
                return sw.GetStringBuilder().ToString();
            }
        }


        public static DeployContent GetObjectBySerializeString(string serializeString)
        {
            DeployContent obj = null;
            JsonSerializer serializer = new JsonSerializer();
            using (StringReader reader = new StringReader(serializeString))
            {
                try
                {
                    obj = serializer.Deserialize<DeployContent>(new JsonTextReader(reader));
                }
                catch
                {
                    throw;
                }
                return obj;
            }
        }

    }
}
