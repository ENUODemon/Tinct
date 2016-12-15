using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Node;
using System.Configuration;
using System.IO;
using Tinct.Net.Communication.Node;

namespace Tinct.Net.Communication.Master
{
    public class TinctMasterNode : MasterNode
    {

        private int masterPort = int.Parse(ConfigurationManager.AppSettings["MasterPort"]);
        private int slaveport = int.Parse(ConfigurationManager.AppSettings["SlavePort"]);
        private Timer blueTimer;


        public static TinctMasterNode Current { get; private set; }
        public List<IMessageHandler> MessageHandlers { get; set; }
        public List<IUnConnectNodeHandler> UnConnectHandlers { get; set; }

        static TinctMasterNode()
        {
            Current = new TinctMasterNode();
        }

        public TinctMasterNode()
        {

            tinctCon = new TinctConnect();
            MessageHandlers = new List<IMessageHandler>();
            UnConnectHandlers = new List<IUnConnectNodeHandler>();
            Current = this;

        }

        public TinctMasterNode(ITinctConnect tinctCon)
        {
            this.tinctCon = tinctCon;
            MessageHandlers = new List<IMessageHandler>();
            UnConnectHandlers = new List<IUnConnectNodeHandler>();
            Current = this;
        }


        private void ReciveConMessage(object sender, ReceiveMessageArgs e)
        {
            
            foreach (var handler in MessageHandlers)
            {
                try
                {
                    if (!handler.HanderMessage(e.ReceivedMessage))
                    {
                        break;
                    }
                }
                catch
                {

                }
            }

        }

        public bool SendMessageToSlave(string message, string machineName)
        {
            return tinctCon.SendMessage(machineName, slaveport, message);
        }



        public override bool StartMaster()
        {
            tinctCon.MessageHandlers += ReciveConMessage;
            IMessageHandler dhandler = new MDeployMessageHandler();
            MessageHandlers.Add(dhandler);
            IMessageHandler handler = new MMessageHander();
            MessageHandlers.Add(handler);

            tinctCon.MachineCloseConnectHandlers += TinctCon_MachineCloseConnectHandlers;

            return tinctCon.ListenningPort(masterPort);

        }

        private void TinctCon_MachineCloseConnectHandlers(object sender, ReceiveMessageArgs e)
        {
            foreach (var handler in UnConnectHandlers)
            {
                try
                {
                    if (!handler.HandleUnConnectNode(e.ReceivedMessage))
                    {
                        break;
                    }
                }
                catch
                {

                }
            }
        }

        public override bool EndMaster()
        {
            return tinctCon.StopListenning();
        }



    }
}
