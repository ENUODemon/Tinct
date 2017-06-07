using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Common.Extension;

namespace Tinct.Net.Message.Message
{
    public class PackageMessage
    {
        public long SendTimeStamp { get; set; }
        public long ReceviceTimeStamp { get; set; }
        public string SourceName { get; set; }
        public string DestinationName { get; set; }
        public TinctMessage Message { get; set; }

        public static PackageMessage GetObjectBySerializeString(string serializeString)
        {
            PackageMessage obj = null;
            JsonSerializer serializer = new JsonSerializer();
            StringReader reader = new StringReader(serializeString);
            try
            {
                obj = serializer.Deserialize<PackageMessage>(new JsonTextReader(reader));
            }
            catch
            {
            }
            return obj;
        }


        public string ToJsonSerializeString()
        {
            JsonSerializer serializer = new JsonSerializer();
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);


            return sw.GetStringBuilder().ToString();
        }
    }
}
