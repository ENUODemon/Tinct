using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Node;
using Tinct.Net.Message.Task;

namespace Tinct.Net.MessageDispath.Controller
{
    public class UrlActionResult : ActionResult
    {


        private List<string> exceptionStringLists = new List<string>();

        public string RemainTaskData { get; set; }

        public List<string> ExceptionStringLists
        {
            get
            {
                return exceptionStringLists;
            }
            set
            {
                exceptionStringLists = value;
            }
        }


        public override void ExecuteResult(ControllerContext context)
        {

            var dispathTaskInfo = context.RequestContext.RouteData.TinctTaskInfo;
            dispathTaskInfo.EndTime = DateTime.Now;
            if (ExceptionStringLists.Count > 0)
            {
                dispathTaskInfo.Status = TinctTaskStatus.Exception;
                foreach (var str in ExceptionStringLists) 
                {
                     dispathTaskInfo.Context.ExceptionOrFaultStrings+="[[["+str+"]]]";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(RemainTaskData))
                {
                    
                    dispathTaskInfo.Status = TinctTaskStatus.Completed;
                }
                else
                {
                    dispathTaskInfo.Status = TinctTaskStatus.PartCompleted;
                }
            }

        }
    }
}
