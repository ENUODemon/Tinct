using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.Interface;

namespace Tinct.TinctTaskMangement.TinctWork
{
    public class TinctTaskEventArgs :EventArgs
    {
       public TinctTaskContext Context { get; set; }
    }
}
