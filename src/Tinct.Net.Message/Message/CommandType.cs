using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Message.Message
{
    public enum CommandType
    {
        Run,
        Cancel,
        Search,
        SyncNode,
        SyncTask,
        Return,
        Deploy
    }
}
