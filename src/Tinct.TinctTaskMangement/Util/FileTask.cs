using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.TinctTaskMangement.Util
{
    public  class FileTask
    {
        public string FileName { get; set;}

        public string SourcePath { get; set; }

        public List<Byte> Content { get; set; } = new List<byte>();

        public static FileTask GetObjectBySerializeString(string serializeString)
        {
            FileTask obj = null;
            JsonSerializer serializer = new JsonSerializer();
            using (StringReader reader = new StringReader(serializeString))
            {
                try
                {
                    obj = serializer.Deserialize<FileTask>(new JsonTextReader(reader));
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
