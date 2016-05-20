using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Task;
using Tinct.Net.MessageDispath.Controller;
using Tinct.Net.MessageDispath.Controller.InterFace;
using Tinct.Net.MessageDispath.Route;
using Tinct.Net.MessageDispath.Route.Interface;

namespace Tinct.Net.MessageDispath.Route
{
 
    public class UrlRouteHandler : MarshalByRefObject, IRouteHandler 
    {


        public RouteData MapToControllerExcute(RouteData data)
        {

            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);

            RequestContext context = new RequestContext(data);
  

            IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            try
            {
                IController controller = controllerFactory.CreateController(context, data.Controller);
                controller.Execute(context);
            }
            catch (AppDomainUnloadedException e) 
            {

            }
            catch (Exception e)
            {
                data.TinctTaskInfo.Status = TinctTaskStatus.Exception;


                data.TinctTaskInfo.Context.ExceptionOrFaultStrings += "[[[" + e.Message + "]]]";
            }
            return data;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
