using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.MessageDispath.Controller;
using Tinct.Net.MessageDispath.Controller.InterFace;

namespace Tinct.Net.MessageDispath.Controller
{
    public class DefaultControllerFactory : IControllerFactory
    {
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController result = null;

            string controllerType = controllerName + "Controller";
            /// TODO.. config it
            Assembly assembly = Assembly.Load("Tinct.PlatformController");
            if (assembly == null) 
            {
                /// TODO.. config it 
                throw new DllNotFoundException("Do not find Tinct.PlatformController assembly!");
            }
            var type = assembly.GetType("Tinct.PlatformController."+controllerType, false);

            if (null != type)
            {
                result = (IController)Activator.CreateInstance(type);
            }
            else 
            {
                /// TODO.. config it 
                throw new ArgumentException("Don not find ControllerName");
            }
            return result;
        }
    }
}
