using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Communication.Cfg
{
    public static class TinctNodeCongratulations
    {
        public static string MasterName
        {
            get
            {
                return ConfigurationManager.AppSettings["MasterName"].ToString();
            }
            private set { }
        }

        public static int MasterPort
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MasterPort"].ToString());
            }
            private set { }

        }

        public static int SlavePort
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["SlavePort"].ToString());
            }
            private set { }

        } 

    }
}
