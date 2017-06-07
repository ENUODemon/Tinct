using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Util;

namespace Tinct.TinctTaskMangement.Interface
{
   public  interface IExcuteTask
    {
        void ExuteTask(string datas, string loggerName, string loggerFileName);
    }
}
