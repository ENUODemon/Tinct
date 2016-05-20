using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.MessageDispath.Controller.InterFace;

namespace Tinct.Net.MessageDispath.Controller
{
    public class ControllerBuilder
    {
        private Func<IControllerFactory> factoryThunk;
        static ControllerBuilder()
        {

            Current = new ControllerBuilder();
           
        }
        public ControllerBuilder()
        {
            this.DefaultNamespaces = new HashSet<string>();
            //
        }
        public static ControllerBuilder Current { get; private set; }
        public IControllerFactory GetControllerFactory()
        {
            return factoryThunk();
        }
        public void SetControllerFactory(IControllerFactory controllerFactory)
        {
            factoryThunk = () => controllerFactory;
        }
        public HashSet<string> DefaultNamespaces { get;  set; }

    }
}
