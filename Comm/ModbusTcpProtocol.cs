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

        public void ModbusTcpOpen()
        {
            _transportSocket.TransportSocketOpen();
        }

        public void ModbusTcpClose()
        {
            _transportSocket.TransportSockClose();
        }

        public int ReadRegister(byte id, ushort beginAddress, ushort number, out List<ushort> data)
        {
            data = new List<ushort>();

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

            int receiveLength = _transportSocket.TransportSocketData(ref sendData, out receiveData);

            if (receiveLength > 0)
            {
                if (receiveData[7] == 3)
                {
                    if (receiveData[8] / 2 == number)
                    {
                        for (int i = 0; i < number; i++)
                        {
                            byte[] dataTwoBytes = new[] {receiveData[9 + 2 * i + 1], receiveData[9 + 2 * i]};
                            data.Add(BitConverter.ToUInt16(dataTwoBytes,0));
                        }
                    }
                }
                else
                {
                    return (receiveData[7] << 8) | receiveData[8];
                }
            }

            return 0;
        }

        public int WriteRegiset(byte id, ushort beginAddress, ref List<ushort> data)
        {
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
            sendData.Add((byte) (data.Count * 2));


            for (int i = 0; i < data.Count; i++)
            {
                byte[] dataBytes = BitConverter.GetBytes(data[i]);
                sendData.Add(dataBytes[1]);
                sendData.Add(dataBytes[0]);
            }

            ushort length = (ushort)(sendData.Count - 6);
            byte[] lengthBytes = BitConverter.GetBytes(length);
            sendData[4] = lengthBytes[1];
            sendData[5] = lengthBytes[0];

            int receiveLength = _transportSocket.TransportSocketData(ref sendData, out receiveData);

            if (length > 0)
            {
                if (receiveData[7] == 0x10)
                {
                    byte[] dataTwoBytes = new[] { receiveData[11], receiveData[10] };

                    if (BitConverter.ToUInt16(dataTwoBytes,0) == data.Count)
                    {
                        ;
                    }
                }
                else
                {
                    return (receiveData[7] << 8) | receiveData[8];
                }
            }

            return 0;
        }
    }
}
