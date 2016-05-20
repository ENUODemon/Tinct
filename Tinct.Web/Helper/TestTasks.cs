using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.Web.Helper
{
    public class TestTasks
    {
        public void GennerateTinctTasks()
        {
            GennerateTestTask();
            //GennerateApiCallTasks(null, null);
            //tre.AddTinctTask(GennerateArchiveEventsTask());
        }
        private void GennerateTestTask()
        {
            TinctTask t1 = new TinctTask();
            t1.Status = TinctTaskStatus.WaittingToRun;
            t1.Context.TaskData = "";
            t1.Context.ControllerName = "TinctTest";
            t1.Context.ActionName = "LoadData1";
            t1.TinctTaskCompleted += null;
        }
    }
}