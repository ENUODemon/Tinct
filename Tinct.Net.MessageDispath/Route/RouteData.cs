using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Message;
using Tinct.Net.Message.Task;
using Tinct.Net.MessageDispath.Route.Interface;

namespace Tinct.Net.MessageDispath.Route
{
 
    [Serializable]
    public class RouteData:MarshalByRefObject
    {
        public IDictionary<string, object> Values { get; private set; }
        public IRouteHandler RouteHandler { get; set; }
        public RouteBase Route { get; set; }

        public RouteData() 
        {
            Values = new Dictionary<string, object>();
        }

  
        public string Controller
        {
            get
            {
                object controllerName = string.Empty;
                this.Values.TryGetValue("controller", out controllerName);
                if (controllerName == null) 
                {
                    return "";
                }

                return controllerName.ToString();
            }
        }
        public string ActionName
        {
            get
            {
                object actionName = string.Empty;
                this.Values.TryGetValue("action", out actionName);
                if (actionName == null)
                {
                    return "";
                }
                return actionName.ToString();
            }
        }

        public Guid TaskID
        {
            get
            {
                object taskID = string.Empty;
                this.Values.TryGetValue("taskID", out taskID);
                return new Guid( taskID.ToString());
            }
        }


        public TinctTaskInfo TinctTaskInfo
        {
            get
            {
                object tinctTaskInfo = null;
                this.Values.TryGetValue("tinctTaskInfo", out tinctTaskInfo);
                return (TinctTaskInfo)tinctTaskInfo;
            }
           

        }

        public CommandType CommandType 
        {
            get
            {
                object commandType = null;
                this.Values.TryGetValue("commandType", out commandType);
                return (CommandType)commandType;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
