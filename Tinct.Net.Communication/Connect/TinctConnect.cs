using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using System.Collections.Concurrent;
using System.Configuration;

namespace Tinct.Net.Communication.Connect
{
    public class TinctConnect : ITinctConnect
    {
        #region Filed
        private TcpListener server = null;

        private ConcurrentDictionary<string, TcpClient> dicTcpSendClients = new ConcurrentDictionary<string, TcpClient>();
        private ConcurrentDictionary<string, TcpClient> dicTcpAcceptClients = new ConcurrentDictionary<string, TcpClient>();
        #endregion


        #region Event
        public event EventHandler<ReceiveMessageArgs> MachineCloseConnectHandlers;

        public event EventHandler<ReceiveMessageArgs> MessageHandlers;

        private void RaiseHanlerMessage(ReceiveMessageArgs e)
        {
            if (MessageHandlers != null)
            {
                MessageHandlers(this, e);
            }
        }

        private void RaiseMachineCloseConnecHanlerMessage(ReceiveMessageArgs e)
        {
            if (MachineCloseConnectHandlers != null)
            {
                MachineCloseConnectHandlers(this, e);
            }
        }


        #endregion





        public bool ListenningPort(int port)
        {

            IPAddress myIPAddress = null;
            IPAddress[] myIPAddresses = (IPAddress[])Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var add in myIPAddresses)
            {
                if (add.AddressFamily == AddressFamily.InterNetwork)
                {
                    //filter some ip address want to improve
                    if (add.ToString().StartsWith("169.254"))
                    {
                        continue;
                    }
                    myIPAddress = add;
                }
            }
            if (myIPAddress == null)
            {
                //log "IPAdress is null ,please check network"
                return false;
            }
            Console.WriteLine("IPAddress is {0}", myIPAddress.ToString());
            var task = StartListenningTaskAsync(myIPAddress, port);

            return !task.IsFaulted;
        }

        public bool StopListenning()
        {
            if (server != null)
            {
                server.Stop();
            }
            return true;
        }

        public void StopAllTCPClinets()
        {
            foreach (var item in dicTcpSendClients)
            {
                item.Value.Close();
            }
            foreach (var item in dicTcpAcceptClients)
            {
                item.Value.Close();
            }

        }

        public bool SendMessage(string machineName, int port, string message)
        {
            TcpClient tcpClient = null;
            var keyString = machineName + ":" + port;
            dicTcpSendClients.TryGetValue(keyString, out tcpClient);
            try
            {
                if (tcpClient == null)
                {
                    tcpClient = new TcpClient(machineName, port);
                    dicTcpSendClients.TryAdd(keyString, tcpClient);
                }
                if (tcpClient.Connected == false)
                {
                    dicTcpSendClients.TryRemove(keyString, out tcpClient);
                }

                Byte[] data = System.Text.Encoding.Unicode.GetBytes(message);

                NetworkStream stream = tcpClient.GetStream();

                stream.Write(data, 0, data.Length);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;

        }

        private async Task StartListenningTaskAsync(IPAddress address, int port)
        {
            new Task(() =>
            {
                try
                {
                    server = new TcpListener(address, port);
                    server.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    server.Stop();
                }
                while (true)
                {
                    Console.WriteLine("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();


                    new Task(() =>
                    {
                        Console.WriteLine("Connected!");

                        NetworkStream stream = client.GetStream();
                        while (true)
                        {
                            Byte[] bytes = new Byte[8192 * 10];
                            int readStep = 0;
                            try
                            {
                                ///if length>81920 have some problem
                                readStep = stream.Read(bytes, 0, bytes.Length);
                            }
                            catch (IOException e)
                            {

                                ////want to improve
                                Console.WriteLine(e.Message);
                                var str = client.Client.RemoteEndPoint.ToString();
                                var remoteIpaddress = str.Substring(0, str.IndexOf(':'));
                                var hostName = Dns.GetHostEntry(remoteIpaddress).HostName;
                                var remoteName = hostName.Substring(0, hostName.IndexOf('.'));
                                Console.WriteLine(remoteName);
                                stream.Close();
                                client.Close();

                                TcpClient clienttcp = null;
                                if (dicTcpAcceptClients.Count > 0)
                                {
                                    var m1 = dicTcpSendClients.Where(m => m.Key.Contains(remoteName)).First();
                                }
                            
                                m1.Value.Close();
                                dicTcpSendClients.TryRemove(m1.Key, out clienttcp);

                                new Task(() =>
                                {
                                    string message = remoteName;
                                    ReceiveMessageArgs args = new ReceiveMessageArgs();
                                    args.ReceivedMessage = message;
                                    RaiseMachineCloseConnecHanlerMessage(args);
                                }).Start();


                                return;
                            }

                            var data = System.Text.Encoding.Unicode.GetString(bytes, 0, readStep);
                            new Task(() =>
                            {
                                string message = data;
                                ReceiveMessageArgs args = new ReceiveMessageArgs();
                                args.ReceivedMessage = message;
                                RaiseHanlerMessage(args);

                            }).Start();
                        };
                    }).Start();
                }
            }).Start();
        }
    }

}
