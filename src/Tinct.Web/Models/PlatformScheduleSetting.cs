using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tinct.Web.Models
{
    public class PlatformScheduleSetting
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string PlatformName { get; set; }
        public DateTime? LastRunTime { get; set; }
        public DateTime? NextRunTime { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ExactRunTime { get; set; }
        public int Interval { get; set; }

        public event EventHandler<PlatformEventArgs> RunTaskEvent;
        public void RaiseRunTaskEvent()
        {
            if (RunTaskEvent != null)
            {
                RunTaskEvent(this, null);
            }
        }
    }
}