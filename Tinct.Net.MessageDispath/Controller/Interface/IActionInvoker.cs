using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tinct.Net.MessageDispath.Controller;

namespace Tinct.Net.MessageDispath.Controller.InterFace
{
    public interface IActionInvoker
    {
        void InvokeAction(ControllerContext controllerContext, string actionName);
    }
}
