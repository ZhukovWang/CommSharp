using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Comm
{
    internal class BaseSocket
    {
        public BaseSocket(string remoteIp, int remotrPort, string localIp, int localPort)
        {
            RemoteIp = remoteIp;
            RemotePort = remotrPort;
            LocalIp = localIp;
            LocalPort = localPort;
            _clinetSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private string _remoteIp;

        public string RemoteIp
        {
            set { _remoteIp = value; }
            get { return _remoteIp; }
        }

        private string _localIp;

        public string LocalIp
        {
            set { _localIp = value; }
            get { return _localIp; }
        }

        private int _remotePort;

        public int RemotePort
        {
            set { _remotePort = value; }
            get { return _remotePort; }
        }

        private int _localPort;

        public int LocalPort
        {
            set { _localPort = value; }
            get { return _localPort; }
        }

        private Socket _clinetSocket;

        public void Init()
        {
            _clinetSocket.Bind(new IPEndPoint(IPAddress.Parse(_localIp), _localPort));
        }

        public void Open()
        {
            _clinetSocket.Connect(IPAddress.Parse(_remoteIp), _remotePort);
        }

        public void Send(ref List<Byte> data)
        {
            byte[] sendData = data.ToArray();
            _clinetSocket.Send(sendData);
        }

        public int Receive(out List<Byte> data)
        {
            data = new List<byte>();
            byte[] recvData = new byte[2048];
            int recvLength = 0;
            recvLength = _clinetSocket.Receive(recvData);
            if (recvLength > 0)
            {
                for (int i = 0; i < recvLength; i++)
                {
                    data.Add(recvData[i]);
                }
            }
            return recvLength;
        }

        public void Close()
        {
            _clinetSocket.Shutdown(SocketShutdown.Both);
            _clinetSocket.Dispose();
            _clinetSocket.Close();
        }
    }
}