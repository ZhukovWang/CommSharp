using Comm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleComm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string localIp = "192.168.1.222";
            int localPort = 0;
            string remoteIp = "192.168.1.2";
            int remotePort = 502;

            int error = 0;

            int length = 10;
            List<ushort> testData = new List<ushort>();

            for (int i = 0; i < length; i++)
            {
                testData.Add((ushort)(0xFFF0 | i));
            }

            Comm.ModbusTcpProtocol modbus = new ModbusTcpProtocol(remoteIp, remotePort, localIp, localPort);

            Console.WriteLine("Opening...");
            error = modbus.ModbusTcpOpen();
            if (error == 0)
            {
                Console.WriteLine("Open success!");
                error = TestWrite(ref modbus, ref testData);
                if (error == 0)
                {
                    error = TestRead(ref modbus, ref length, ref testData);
                }
                Console.WriteLine("Closing...");
                error = modbus.ModbusTcpClose();
                if (error == 0)
                {
                    Console.WriteLine("Close success!");
                }
                else
                {
                    Console.WriteLine("Close fail. Error code is 0x" + Convert.ToString(error, 16).ToUpper() + ".");
                }

            }
            else
            {
                Console.WriteLine("Open fail. Error code is 0x" + Convert.ToString(error, 16).ToUpper() + ".");
            }

            Console.Read();
        }

        private static int TestWrite(ref ModbusTcpProtocol modbus, ref List<ushort> data)
        {
            Console.WriteLine("Test write...");
            Console.Write("Write data is:");
            foreach (var i in data)
            {
                Console.Write(" " + i.ToString());
            }
            Console.WriteLine();

            int error = 0;
            error = modbus.WriteRegiset(1, 0x000, ref data);
            if (error == 0)
            {
                Console.WriteLine("Write success!");
            }
            else
            {
                Console.WriteLine("Write fail. Error code is " + error.ToString() + ".");
            }

            return error;
        }

        private static int TestRead(ref ModbusTcpProtocol modbus, ref int length, ref List<ushort> data)
        {
            Console.WriteLine("Test read...");
            int error = 0;
            List<ushort> readData = new List<ushort>();
            error = modbus.ReadRegister(1, 0x0000, (ushort)length, ref readData);
            if (error == 0)
            {
                Console.Write("Read data is:");
                foreach (var i in readData)
                {
                    Console.Write(" " + i.ToString());
                }
                Console.WriteLine();
                Console.WriteLine("Read success!");
                if (data.SequenceEqual(readData))
                {
                    Console.WriteLine("Read data is same as send!");
                }
                else
                {
                    Console.WriteLine("Read data is NOT same as send!");
                }
            }
            else
            {
                Console.WriteLine("Read fail. Error code is 0x" + Convert.ToString(error, 16).ToUpper() + ".");
            }

            return error;
        }
    }
}