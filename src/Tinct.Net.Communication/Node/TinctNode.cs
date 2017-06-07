using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Message;

namespace Tinct.Net.Communication.Node
{
    public class TinctNode
    {
        protected IConnect connect = new TinctConnect();

        public int HandlerMessageCount { get; set; } = 1;

        public NodeInfo NodeInfo { get; set; } = new NodeInfo() { NodeName = Dns.GetHostName() };

        public List<IMessageHandler> MessageHandlers { get; set; } = new List<IMessageHandler>();

        public Action<PackageMessage> RaisePackageMessage { get; set; } = null;

        public TinctNode() { }

        public TinctNode(IConnect newconnect)
        {
            connect = newconnect;
        }

        protected virtual bool SendMessage(string machineName, int port, PackageMessage message)
        {
            return connect.SendMessage(machineName, port, message);
        }

        protected virtual void HandleReceviceMessage()
        {
            for (int i = 0; i < HandlerMessageCount; i++)
            {
                new Task(() =>
                {
                    PackageMessage receviceMessage = null;
                    do
                    {
                        receviceMessage = connect.GetReceviceMessage();
                        if (RaisePackageMessage != null)
                        {
                            RaisePackageMessage(receviceMessage);
                        }

                        try
                        {
                            foreach (var handler in MessageHandlers)
                            {
                                if (!handler.HanderMessage(receviceMessage.Message))
                                {
                                    break;
                                }
                            }
                        }
                        catch
                        {

                        }
                    } while (receviceMessage != null);
                }).Start();
            }

        }

        protected virtual bool StartReceviceMessageService(int port)
        {
            return connect.ReceviceMessage(port);
        }

        protected virtual void CloseConnectResource()
        {
            connect.CloseConnectResource();
        }
    }
}
