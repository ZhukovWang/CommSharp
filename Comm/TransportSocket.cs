using System;
using System.Collections.Generic;

namespace Comm
{
    internal class TransportSocket
    {
        public TransportSocket(string remoteIp, int remotrPort, string localIp, int localPort)
        {
            _baseSocket = new BaseSocket(remoteIp, remotrPort, localIp, localPort);
        }

        private BaseSocket _baseSocket;

        public void TransportSocketOpen()
        {
            _baseSocket.Init();

            _baseSocket.Open();
        }

        public int TransportSocketData(ref List<Byte> sendData, out List<Byte> receiveData)
        {
            receiveData = new List<byte>();
            _baseSocket.Send(ref sendData);
            int length = _baseSocket.Receive(out receiveData);
            return length;
        }

        public void TransportSockClose()
        {
            _baseSocket.Close();
        }
    }
}