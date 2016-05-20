using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Interface;

namespace Tinct.TinctTaskMangement.TinctWork
{
    public class TinctTaskContext : ITinctTaskContext
    {

        public string MachineName { get; set; }

        public Guid TaskID { get; set; }


        public string ActionName
        {
            get;
            

            set;
          
        }

        public string ControllerName
        {
            get;


            set;
           
        }

        public string TaskData
        {
            get;
          
            set; 
        }

    }
}
