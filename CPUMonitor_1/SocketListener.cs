using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CPUMonitor_1
{
    public class SocketListener
    {

        private CPUMonitor monitor;
        private Thread monitorThread;

        public SocketListener(CPUMonitor monitor)
        {
            this.monitor = monitor;
        }

        // Incoming data from the client.
        public string data = null;

        public void startListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    // Show the data on the console.
                    Console.WriteLine("Text received : {0}", data);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();


                    if (data.StartsWith("start"))
                    {
                        this.startMonitoring(data);
                    }
                    else if (data.StartsWith("stop"))
                    {
                        this.stopMonitoring();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public void startMonitoring(string command)
        {
            //this.monitorThread = new Thread(new ThreadStart(CPUMonitor.ThreadProc));
            command = command.Replace("<EOF>", "");
            string[] parameters = command.Split(' ');
            int interval = Convert.ToInt32(parameters[1]);
            string filename = parameters[2];
            LinkedList<string> processes = new LinkedList<string>();
            for (int i = 3; i < parameters.Length; i++ )
            {
                processes.AddLast(parameters[i]);
            }
            monitor.setInterval(interval);
            monitor.setFilename(filename);
            monitor.setProcess(processes);
            this.monitorThread = new Thread(new ThreadStart(this.monitor.ThreadProc));
            this.monitorThread.Start();
        }

        public void stopMonitoring()
        {
            this.monitor.stop();
        }

        public static int Main(String[] args)
        {

            CPUMonitor monitor = new CPUMonitor();
            SocketListener listener = new SocketListener(monitor);
            listener.startListening();
            return 0;
        }
    }
}