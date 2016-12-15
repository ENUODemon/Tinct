using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tinct.Net.Message.Extension
{
    public  class IsoDateTimeConverterExtension: DateTimeConverterBase
    {
        private  IsoDateTimeConverter dtConverter = new IsoDateTimeConverter();

        public IsoDateTimeConverterExtension(string format)
        {
            dtConverter.DateTimeFormat = format;
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }


    }
}
