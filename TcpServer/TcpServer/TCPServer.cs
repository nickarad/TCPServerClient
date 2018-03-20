

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SouthBoundAPI;
using Newtonsoft.Json;

namespace SampleTCPServer
{
    public class TCPServer
    {
        #region Variables
        //------------------------------------------------------------------------------------------------------------------------
        public bool Started = false;
        Socket Server;
        int Port;
        bool IsRunning = false;
        private Thread _ListenerThread = null;
        private List<Socket> _ConnectedClients = new List<Socket>();
        IoTNode newNode = new IoTNode();
        //------------------------------------------------------------------------------------------------------------------------
        #endregion

        #region Constructor
        //------------------------------------------------------------------------------------------------------------------------
        public TCPServer(int Port = 8081)
        {
            this.Port = Port;
        }
        #endregion

        #region Functions
        //------------------------------------------------------------------------------------------------------------------------
        public void Start()
        {
            lock (this)
            {
                //set flag
                IsRunning = true;

                //start listener
                _ListenerThread = new Thread(new ParameterizedThreadStart(ServerThread));
                _ListenerThread.IsBackground = true;
                _ListenerThread.Start(this.Port);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------

        public void Stop()
        {
            if (IsRunning)
            {
                lock (this)
                {
                    //close server
                    IsRunning = false;
                    try
                    {
                        _ListenerThread.Join();
                        _ListenerThread.Abort();
                    }
                    finally { _ListenerThread = null; }
                }
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        private void ServerThread(object Port)
        {
            Server = null;
            try
            {
                try
                {
                    //start server
                    Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Server.Bind(new IPEndPoint(IPAddress.Any, this.Port));
                    Server.Listen(10);
                    Console.WriteLine("Server is listening on: " + this.Port.ToString());
                    //wait for new clients
                    while (IsRunning)
                    {
                        try
                        {
                            var client = Server.Accept();
                            _ConnectedClients.Add(client);
                            Task.Run(() => { ClientChannel(client); });
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            catch { }
            finally
            {
                try { Server?.Disconnect(false); } catch { }
                try { Server?.Close(); } catch { }
                try { Server?.Dispose(); } catch { }
            }

            this.Stop();
        }
        //------------------------------------------------------------------------------------------------------------------------
        private void ClientChannel(object client)
        {
            //get client socket
            var socket = (Socket)client;
            Console.WriteLine((string.Format("New client from " + socket.RemoteEndPoint.ToString())));

            using (var netStream = new NetworkStream(socket, false))
            using (var reader = new StreamReader(netStream))
            {
                while (true)
                {
                    byte[] b = new byte[512];
                    int k = socket.Receive(b);
                    string req = Encoding.ASCII.GetString(b, 0, k);
                    newNode= JsonConvert.DeserializeObject<IoTNode>(req); //Desirialize Object  
                    Console.WriteLine("Name: " + newNode.Name);
                    Console.WriteLine("State: " + newNode.State);
                    Console.WriteLine("SerialNumber: " + newNode.SerialNumber);
                    Console.WriteLine("Manufacturer: " + newNode.Manufacturer);
                    Console.WriteLine("Firmware: " + newNode.Firmware);
                    Console.WriteLine("Things: " + newNode.Things);

                    //Console.WriteLine("Rx: " + req);
                    try
                    {

                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        //------------------------------------------------------------------------------------------------------------------------

    }
    #endregion
}
