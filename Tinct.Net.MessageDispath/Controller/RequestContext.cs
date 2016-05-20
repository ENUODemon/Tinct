using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tinct.Net.Message;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Task;
using Tinct.Net.MessageDispath.Route;

namespace Tinct.Net.MessageDispath.Controller
{
    public class RequestContext:MarshalByRefObject
    {
        public virtual RouteData RouteData { get; set; }

        public RequestContext() { }
        public RequestContext(RouteData RouteData) 
        {
            this.RouteData = RouteData;
        }

    }
}
