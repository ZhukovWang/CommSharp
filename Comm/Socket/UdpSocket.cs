using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Comm.Socket
{
    internal class UdpSocket
    {
        public UdpSocket(string remoteIp, int remotePort, string localIp, int localPort)
        {
            RemoteIp = remoteIp;
            RemotePort = remotePort;
            LocalIp = localIp;
            LocalPort = localPort;
            localEndPoint = new IPEndPoint(IPAddress.Parse(_localIp), _localPort);
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(_remoteIp), _remotePort);
            recvEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        private string _remoteIp;

        public string RemoteIp
        {
            set
            {
                _remoteIp = value;
            }

            get
            {
                return _remoteIp;
            }
        }

        private string _localIp;

        public string LocalIp
        {
            set
            {
                _localIp = value;
            }

            get
            {
                return _localIp;
            }
        }

        private int _remotePort;

        public int RemotePort
        {
            set
            {
                _remotePort = value;
            }

            get
            {
                return _remotePort;
            }
        }

        private int _localPort;

        public int LocalPort
        {
            set
            {
                _localPort = value;
            }

            get
            {
                return _localPort;
            }
        }

        private UdpClient _udpClient;

        private IPEndPoint localEndPoint;
        private IPEndPoint remoteEndPoint;
        private IPEndPoint recvEndPoint;

        public int Init()
        {
            try
            {
                _udpClient = new UdpClient(localEndPoint);
                _udpClient.Client.SendTimeout = 3000;
                _udpClient.Client.ReceiveTimeout = 3000;
            }
            catch
            {
                throw;
            }

            return 0;
        }

        public int Send(ref List<Byte> data)
        {
            byte[] sendData = data.ToArray();

            try
            {
                _udpClient.Send(sendData, sendData.Length, remoteEndPoint);
            }
            catch
            {
                throw;
            }

            return 0;
        }

        public int Receive(out List<Byte> data)
        {
            data = new List<byte>();
            byte[] recvData = new byte[2048];

            try
            {
                recvData = _udpClient.Receive(ref recvEndPoint);
            }
            catch
            {
                return 2;
            }

            if (recvData.Length > 0)
            {
                foreach (byte b in recvData)
                {
                    data.Add(b);
                }

                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
