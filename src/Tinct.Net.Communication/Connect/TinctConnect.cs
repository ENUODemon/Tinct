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
using System.Threading;
using Tinct.Net.Message.Message;
using Tinct.Common.Extension;

namespace Tinct.Net.Communication.Connect
{
    public class TinctConnect : IConnect
    {
        private ConcurrentDictionary<string, TcpClient> dicTcpSendClients = new ConcurrentDictionary<string, TcpClient>();
        private ConcurrentDictionary<string, TcpClient> dicTcpAcceptClients = new ConcurrentDictionary<string, TcpClient>();
        private TcpListener listener = null;
        private ConcurrentQueue<PackageMessage> MessageQueues = new ConcurrentQueue<PackageMessage>();

        private EventWaitHandle eventqueue = new EventWaitHandle(false, EventResetMode.ManualReset, Guid.NewGuid().ToString());

        public TinctConnect()
        {
        }

        public bool SendMessage(string machineName, int port, PackageMessage message)
        {
            var keyString = machineName + ":" + port;
            dicTcpSendClients.TryGetValue(keyString, out TcpClient tcpClient);
            try
            {
                if (tcpClient == null)
                {
                    tcpClient = new TcpClient(machineName, port);
                    dicTcpSendClients.TryAdd(keyString, tcpClient);
                }
                else if (tcpClient.Connected == false)
                {
                    dicTcpSendClients.TryRemove(keyString, out tcpClient);
                    return false;
                }
                Byte[] data = System.Text.Encoding.Unicode.GetBytes(message.ToJsonSerializeString());
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

        public PackageMessage GetReceviceMessage()
        {
            if (MessageQueues.Count == 0)
            {
                eventqueue.WaitOne();
            }
            MessageQueues.TryDequeue(out PackageMessage result);

            return result;
        }

        public bool ReceviceMessage(int port)
        {
            var ipAddress = GetAvalibleAddress();
            if (ipAddress == null)
            {
                //log it 
                Console.WriteLine("Do not get ipaddress,please check network");
                return false;
            }
            listener = StartListen(ipAddress, port);
            if (listener == null)
            {
                //log it 
                return false;
            }
            Task t = new Task(() => { EnQueueMessage(); });
            t.Start();
            return true;
        }

        public void CloseConnectResource()
        {
            StopListenning();
            StopAllTCPClinets();
        }

        private bool StopListenning()
        {
            if (listener != null)
            {
                listener.Stop();
            }
            return true;
        }

        private void StopAllTCPClinets()
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


        private IPAddress GetAvalibleAddress()
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
            return myIPAddress;
        }

        private TcpListener StartListen(IPAddress address, int port)
        {

            TcpListener listener = null;
            try
            {
                listener = new TcpListener(address, port);
                listener.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (listener != null)
                {
                    listener.Stop();
                }
            }
            return listener;
        }

        private string GetRemoteName(TcpClient acceptClient)
        {
            var str = acceptClient.Client.RemoteEndPoint.ToString();
            var remoteIpaddress = str.Substring(0, str.IndexOf(':'));
            var remotehostName = Dns.GetHostEntry(remoteIpaddress).HostName;
            var remoteName = remotehostName.Substring(0, remotehostName.IndexOf('.'));
            return remoteName;
        }

        private void EnQueueMessage()
        {

            while (true)
            {
                Console.WriteLine("Waiting for a connection... ");
                TcpClient acceptClient = listener.AcceptTcpClient();
                new Task(() =>
                {
                    Console.WriteLine("Connected!");
                    NetworkStream stream = acceptClient.GetStream();

                    List<byte> lists = new List<byte>();

                    int readStep = 0;
                    while (true)
                    {
                        Byte[] bytes = new Byte[8192 * 10];
                        try
                        {
                            readStep = stream.Read(bytes, 0, bytes.Length);
                            lists.AddRange(bytes);
                           
                            if (readStep< bytes.Length)
                            {
                               
                                long receviceTime = DateTimeExtension.GetTimeStamp();
                                var data = System.Text.Encoding.Unicode.GetString(lists.ToArray(), 0, lists.Count);
                                var message = PackageMessage.GetObjectBySerializeString(data);
                                if (message != null)
                                {
                                    message.ReceviceTimeStamp = receviceTime;
                                    MessageQueues.Enqueue(message);
                                }
                                else
                                {
                                    ///Filter some message
                                }
                                if (MessageQueues.Count == 1)
                                {
                                    eventqueue.Set();
                                    eventqueue.Reset();
                                }
                                lists.Clear();
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e.Message);
                            stream.Close();
                            acceptClient.Close();

                            var remoteName = GetRemoteName(acceptClient);
                            if (dicTcpAcceptClients.Count > 0)
                            {
                                var m1 = dicTcpSendClients.Where(m => m.Key.Contains(remoteName)).First();
                                m1.Value.Close();
                                dicTcpSendClients.TryRemove(m1.Key, out TcpClient clienttcp);
                                clienttcp = null;
                            }
                            return;
                        }

                    };
                }).Start();
            }
        }


    }

}
