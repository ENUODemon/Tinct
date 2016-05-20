using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.MessageDispath.Controller.InterFace;

namespace Tinct.Net.MessageDispath.Controller
{
    public  static class ControllerCahe
    {
        public static Dictionary<string, IController> controllerCahe = new Dictionary<string, IController>();


    }
}
