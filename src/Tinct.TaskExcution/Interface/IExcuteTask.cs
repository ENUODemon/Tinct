using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.TaskExcution.Interface
{
    public interface IExcuteTask
    {
        void ExuteTask(string datas, string loggerName, string loggerFileName);
    }
}
