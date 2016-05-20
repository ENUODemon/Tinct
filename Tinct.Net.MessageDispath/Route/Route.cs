using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Task;
using Tinct.Net.MessageDispath.Controller;
using Tinct.Net.MessageDispath.Route.Interface;

namespace Tinct.Net.MessageDispath.Route
{
    public class Route : RouteBase
    {
        public IRouteHandler RouteHandler { get; set; }
        public Route()
        {
            this.RouteHandler = new UrlRouteHandler();
            
            Controller.ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory());
            Controller.ControllerBuilder.Current.DefaultNamespaces = new HashSet<string>(new List<string>() { "Tinct.PlatformController" });
        }

        public override RouteData GetRouteData(string urlmessage)
        {
            TinctTaskInfo obj;
            RouteData routeData = new RouteData();
            routeData.Route = this;
            try
            {
                obj = TinctTaskInfo.GetObjectBySerializeString(urlmessage);
            }
            catch
            {
                throw;
            }
            routeData.Values.Add("tinctTaskInfo", obj);

            routeData.Values.Add("controller", obj.Context.ControllerName);
            routeData.Values.Add("action", obj.Context.ActionName);

            routeData.Values.Add("taskID", obj.ID);

            routeData.Values.Add("commandType", obj.Command);
            routeData.RouteHandler = RouteHandler;
            return routeData;
        }
        public override object InitializeLifetimeService()
        {
            return null;
        }

    }
}
