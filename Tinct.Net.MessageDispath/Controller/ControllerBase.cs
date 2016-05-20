using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Task;
using Tinct.Net.MessageDispath.Controller.InterFace;

namespace Tinct.Net.MessageDispath.Controller
{
    public abstract class ControllerBase : IController
    {
        protected IActionInvoker ActionInvoker { get; set; }
        public ControllerBase()
        {
            this.ActionInvoker = new ControllerActionInvoker();
        }
        public void Execute(RequestContext context)
        {
            ControllerContext controllerContext = new ControllerContext { RequestContext = context, Controller = this };
            try
            {
                this.ActionInvoker.InvokeAction(controllerContext, context.RouteData.ActionName);
            }
            catch (Exception e) 
            {
                context.RouteData.TinctTaskInfo.Status = TinctTaskStatus.Exception;
                context.RouteData.TinctTaskInfo.Context.ExceptionOrFaultStrings += "[[[" + "excute has ecception" + "\r\n" + e.Message + "]]]";
            }
        }
    }
}
