using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;

namespace Tinct.Net.Communication.Monitor
{


    public class TinctMasterMonitor:IMasterMonitor
    {
        private int MonitorPort;

        private TinctConnect tinctCon;

        public event EventHandler<ReceiveMessageArgs> ReciveConMessage;

        public event EventHandler<SendMessageArgs> SendMessageAfterRecive;
        public TinctMasterMonitor()
        {
            MonitorPort=int.Parse(ConfigurationManager.AppSettings["MonitorPort"]);
            tinctCon = new TinctConnect();
            
        }

        public bool StartMonitor()
        {

            tinctCon.DispathMessage += ReciveConMessage;
            tinctCon.SendMessageAfterDispathMessage += SendMessageAfterRecive;
            if (tinctCon.ListenningPort(MonitorPort))
            {
                Console.WriteLine("Start Monitor successful");
                return true;
            }
            else
            {
                return false;
            }
        
        }

        public bool EndMonitor()
        {
            return  tinctCon.StopListenning();
        }
    }
}
