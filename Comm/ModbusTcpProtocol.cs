using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comm
{
    public class ModbusTcpProtocol
    {
        public ModbusTcpProtocol(string remoteIp, int remotrPort, string localIp, int localPort)
        {
            _transportSocket = new TransportSocket(remoteIp, remotrPort, localIp, localPort);
        }

        private TransportSocket _transportSocket;
        private ushort _identifier = 0;

        public int ModbusTcpOpen()
        {
            return _transportSocket.TransportSocketOpen();
        }

        public int ModbusTcpClose()
        {
            return _transportSocket.TransportSockClose();
        }

        public int ReadRegister(byte id, ushort beginAddress, ushort number, ref List<ushort> data)
        {
            int error = 0;

            List<byte> sendData = new List<byte>();
            List<byte> receiveData = new List<byte>();

            //此次通信事务处理标识符
            byte[] identifierBytes = BitConverter.GetBytes(_identifier);
            sendData.Add(identifierBytes[1]);
            sendData.Add(identifierBytes[0]);
            _identifier++;

            //协议标识符
            sendData.Add(0);
            sendData.Add(0);

            //数据长度
            sendData.Add(0);
            sendData.Add(6);

            //站号
            sendData.Add(id);

            //功能码
            sendData.Add(0x03);

            //起始地址
            byte[] beginAddressBytes = BitConverter.GetBytes(beginAddress);
            sendData.Add(beginAddressBytes[1]);
            sendData.Add(beginAddressBytes[0]);

            //长度
            byte[] numberBytes = BitConverter.GetBytes(number);
            sendData.Add(numberBytes[1]);
            sendData.Add(numberBytes[0]);

            error = _transportSocket.TransportSocketData(ref sendData, ref receiveData);

            if (error == 0)
            {
                if (receiveData[7] == 3)
                {
                    if (receiveData[8] / 2 == number)
                    {
                        for (int i = 0; i < number; i++)
                        {
                            byte[] dataTwoBytes = new[] {receiveData[9 + 2 * i], receiveData[9 + 2 * i + 1]};
                            data.Add(BitConverter.ToUInt16(dataTwoBytes,0));
                        }
                    }
                    else
                    {
                        return 0x10;
                    }
                }
                else
                {
                    return receiveData[8] | 0x10;
                }
            }

            return error;
        }

        public int WriteRegiset(byte id, ushort beginAddress, ref List<ushort> data)
        {
            int error = 0;

            List<byte> sendData = new List<byte>();
            List<byte> receiveData = new List<byte>();

            //此次通信事务处理标识符
            byte[] identifierBytes = BitConverter.GetBytes(_identifier);
            sendData.Add(identifierBytes[1]);
            sendData.Add(identifierBytes[0]);
            _identifier++;

            //协议标识符
            sendData.Add(0);
            sendData.Add(0);

            //数据长度，组完报文再填写
            sendData.Add(0);
            sendData.Add(0);

            //站号
            sendData.Add(id);

            //功能码
            sendData.Add(0x10);

            //起始地址
            byte[] beginAddressBytes = BitConverter.GetBytes(beginAddress);
            sendData.Add(beginAddressBytes[1]);
            sendData.Add(beginAddressBytes[0]);

            //长度
            byte[] numberBytes = BitConverter.GetBytes((ushort)data.Count);
            sendData.Add(numberBytes[1]);
            sendData.Add(numberBytes[0]);

            foreach (var i in data)
            {
                byte[] dataBytes = BitConverter.GetBytes(i);
                sendData.Add(numberBytes[1]);
                sendData.Add(numberBytes[0]);
            }

            ushort length = (ushort)(sendData.Count - 6);
            byte[] lengthBytes = BitConverter.GetBytes(length);
            sendData[4] = lengthBytes[1];
            sendData[5] = lengthBytes[0];

            error = _transportSocket.TransportSocketData(ref sendData, ref receiveData);

            if (error == 0)
            {
                if (receiveData[7] == 0x10)
                {
                    byte[] dataTwoBytes = new[] { receiveData[10], receiveData[11] };

                    if (BitConverter.ToUInt16(dataTwoBytes,0) / 2 == data.Count)
                    {
                        ;
                    }
                    else
                    {
                        return 0x10;
                    }
                }
                else
                {
                    return receiveData[8] | 0x10;
                }
            }

            return error;
        }
    }
}
