using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.MessageDispath.Route
{
    public abstract  class RouteBase:MarshalByRefObject
    {
        public abstract RouteData GetRouteData(string urlmessage);
    }
}
