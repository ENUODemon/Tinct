using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.MessageDispath.Controller;

namespace Tinct.Net.MessageDispath.Controller.InterFace
{
   public  interface IControllerFactory
    {
        IController CreateController(RequestContext requestContext, string controllerName);
    }
}
