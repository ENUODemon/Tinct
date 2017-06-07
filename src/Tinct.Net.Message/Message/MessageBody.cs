using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Message.Message
{
    [Serializable]
    public class MessageBody:MarshalByRefObject
    {
        public string Datas { get; set; }
    }
}
