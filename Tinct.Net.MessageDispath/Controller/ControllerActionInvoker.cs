using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message;
using Tinct.Net.Message.Task;
using Tinct.Net.MessageDispath.Controller.InterFace;

namespace Tinct.Net.MessageDispath.Controller
{
    public class ControllerActionInvoker : IActionInvoker
    {
        public ControllerActionInvoker()
        {
        }
        public void InvokeAction(ControllerContext controllerContext, string actionName)
        {
            MethodInfo method=null;
            ActionResult actionResult = null;
            try
            {
                method = controllerContext.Controller.GetType().GetMethod(actionName);
            }
            catch(Exception e)
            {
                throw e;
            }
            if (method != null)
            {
                List<object> parameters = new List<object>();
                object tinctTaskInfo;

                controllerContext.RequestContext.RouteData.Values.TryGetValue("tinctTaskInfo", out tinctTaskInfo);
                parameters.Add(((TinctTaskInfo)tinctTaskInfo).Context.TaskData);

                try
                {
                    actionResult = method.Invoke(controllerContext.Controller, parameters.ToArray()) as ActionResult;
                    actionResult.ExecuteResult(controllerContext);
                }
                catch (Exception e)
                {

                    throw new Exception(string.Format("Invoke action Name {0} failed", actionName));
                }

            }
            else { throw new ArgumentException("Not found action!"); }
          
        
        }
    }
}
