using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Common.Log
{
    public   class LogEntity
    {

        private DateTime date;

        public DateTime Date 
        { 
            get { return date == DateTime.MinValue ? DateTime.Now : date; } 
            set { date = value; } 
        }

        public object Message { get; set; }


        public static LogEntity GetObjectBySerializeString(string serializeString) 
        {
            LogEntity obj = null;
            JsonSerializer serializer = new JsonSerializer();
            using (StringReader reader = new StringReader(serializeString))
            {
                try
                {
                    obj = serializer.Deserialize<LogEntity>(new JsonTextReader(reader));
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
