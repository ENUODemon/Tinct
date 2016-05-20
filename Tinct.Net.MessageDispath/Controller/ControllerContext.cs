using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tinct.Net.MessageDispath.Controller
{
   public  class ControllerContext
    {
        public ControllerBase Controller { get; set; }
        public RequestContext RequestContext { get; set; }
    }
}
