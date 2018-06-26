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

        public int TransportSocketOpen()
        {
            int error = _baseSocket.Init();

            if (error == 0)
            {
                error = _baseSocket.Open();
            }

            return error;
        }

        public int TransportSocketData(ref List<Byte> sendData, ref List<Byte> receiveData)
        {
            int error = _baseSocket.Send(ref sendData);
            if (error == 0)
            {
                error = _baseSocket.Receive(ref receiveData);
            }

            return error;
        }

        public int TransportSockClose()
        {
            int error = _baseSocket.Close();
            return error;
        }
    }
}