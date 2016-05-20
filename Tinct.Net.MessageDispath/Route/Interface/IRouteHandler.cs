using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Task;
using Tinct.Net.MessageDispath.Route;

namespace Tinct.Net.MessageDispath.Route.Interface
{
    public interface IRouteHandler
    {
        RouteData MapToControllerExcute(RouteData data);
    }
}
