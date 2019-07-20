using Microsoft.VisualStudio.TestTools.UnitTesting;
using Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comm.Protocol;

namespace Comm.Tests
{
    [TestClass()]
    public class ModbusTcpProtocolTests
    {
        [TestMethod()]
        public void RegisterTest()
        {
            string localIp = "192.168.1.222";
            int localPort = 0;
            string remoteIp = "192.168.1.2";
            int remotePort = 502;

            int length = 10;
            List<ushort> testData = new List<ushort>();

            for (int i = 0; i < length; i++)
            {
                testData.Add((ushort)(0xFFF0 | i));
            }

            ModbusTcpProtocol modbus = new ModbusTcpProtocol(remoteIp, remotePort, localIp, localPort);

            Console.WriteLine("Opening...");
            try
            {
                modbus.ModbusTcpOpen();
            }
            catch (Exception e)
            {
                Console.WriteLine("Open fail. Error is " + e.Message + ".");
                return;
            }

            Console.WriteLine("Open success!");

            TestWrite(ref modbus, ref testData);
            TestRead(ref modbus, ref length, ref testData);

            Console.WriteLine("Closing...");
            try
            {
                modbus.ModbusTcpClose();
            }
            catch (Exception e)
            {
                Console.WriteLine("Close fail. Error is " + e.Message + ".");
                return;
            }

            Console.WriteLine("Close success!");

            Assert.IsTrue(true);
        }

        private void TestWrite(ref ModbusTcpProtocol modbus, ref List<ushort> data)
        {
            Console.WriteLine("Test write...");
            Console.Write("Write data is:");
            foreach (var i in data)
            {
                Console.Write(" " + i.ToString());
            }
            Console.WriteLine();

            int error = 0;
            try
            {
                error = modbus.WriteRegister(1, 0x000, ref data);
                if (error != 0)
                {
                    Console.WriteLine("Write fail. Error code is 0x" + Convert.ToString(error, 16).ToUpper() + ".");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Write fail. Error is " + e.Message + ".");
            }

            Console.WriteLine("Write success!");
        }

        private void TestRead(ref ModbusTcpProtocol modbus, ref int length, ref List<ushort> data)
        {
            Console.WriteLine("Test read...");
            int error = 0;
            List<ushort> readData = new List<ushort>();
            try
            {
                error = modbus.ReadRegister(1, 0x0000, (ushort)length, out readData);
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Read fail. Error is " + e.Message + ".");
            }
        }
    }
}