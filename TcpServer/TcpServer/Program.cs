using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SampleTCPServer;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {

            TCPServer myServer = new TCPServer();
            myServer.Start();

            Console.ReadKey();
        }
    }
}
