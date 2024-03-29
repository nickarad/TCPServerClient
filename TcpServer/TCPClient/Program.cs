﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using SouthBoundAPI;
using Newtonsoft.Json;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                //Registration newReg = new Registration();
                // Create new Node
                IoTNode newNode = new IoTNode();
                newNode.Name = "sensor";
                newNode.State = 15;
                newNode.SerialNumber = "ad123";
                newNode.Manufacturer = "sdsad";
                newNode.Firmware = "firm";


                string output = JsonConvert.SerializeObject(newNode);

                Console.Write(output);

                Console.ReadKey();


                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect("192.168.1.2", 8081);
                // use the ipaddress as in the server program

                Console.WriteLine("Connected");




                //Console.Write("Enter the string to be transmitted : ");

                //String str = Console.ReadLine();

                Stream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(output);
                Console.WriteLine("Transmitting.....");

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(bb[i]));

                tcpclnt.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }

            Console.ReadKey();
        }
    
    }
}
