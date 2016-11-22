using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;

namespace Tinct.Net.Communication.Connect
{
    public class TinctConnect : ITinctConnect
    {
        private TcpListener server = null;
        private void RaiseTaskMessage(ReceiveMessageArgs e)
        {
            if (TaskMessage != null)
            {
                TaskMessage(this, e);
            }
        }

        public event EventHandler<ReceiveMessageArgs> TaskMessage;

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


            var startServerTask = new Task(() =>
            {
                try
                {
                    server = new TcpListener(myIPAddress, port);
                    server.Start();
                    List<byte> messagebytes = new List<byte>();

                    String data = null;
                    while (true)
                    {
                        messagebytes.Clear();
                        Console.WriteLine("Waiting for a connection... ");
                        TcpClient client = server.AcceptTcpClient();



                        new Task(() =>
                        {
                            Console.WriteLine("Connected!");
                            data = null;
                            Byte[] bytes = new Byte[256];

                            using (NetworkStream stream = client.GetStream())
                            {
                                StringBuilder messagebuilder = new StringBuilder();
                                int i;
                                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                    data = System.Text.Encoding.Unicode.GetString(bytes, 0, i);
                                    messagebuilder.Append(data);
                                    bytes = new Byte[256];
                                }
                                new Task(() =>
                                {
                                    string message = messagebuilder.ToString();

                                    ReceiveMessageArgs args = new ReceiveMessageArgs();
                                    args.ReceivedMessage = message;

                                    RaiseTaskMessage(args);

                                }).Start();
                            }
                            client.Close();
                        }).Start();


                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.Message);
                    //log
                    throw;
                }
                finally
                {
                    server.Stop();
                }
            });

            startServerTask.Start();
            return !startServerTask.IsFaulted;

        }

        public bool StopListenning()
        {
            if (server != null)
            {
                server.Stop();
            }
            return true;
        }



        public bool SendMessage(string machineName, int port, string message)
        {

            try
            {

                using (TcpClient client = new TcpClient(machineName, port))
                {
                    Byte[] data = System.Text.Encoding.Unicode.GetBytes(message);
                    using (NetworkStream stream = client.GetStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;

        }



    }
}
