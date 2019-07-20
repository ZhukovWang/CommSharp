using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Comm.Socket
{
    internal class BaseSocket
    {
        public BaseSocket(string remoteIp, int remotePort, string localIp, int localPort)
        {
            RemoteIp = remoteIp;
            RemotePort = remotePort;
            LocalIp = localIp;
            LocalPort = localPort;
            _clientSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSocket.SendTimeout = 3000;
            _clientSocket.ReceiveTimeout = 3000;
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

        private System.Net.Sockets.Socket _clientSocket;

        public void Init()
        {
            _clientSocket.Bind(new IPEndPoint(IPAddress.Parse(_localIp), _localPort));
        }

        public void Open()
        {
            var result = _clientSocket.BeginConnect(IPAddress.Parse(_remoteIp), _remotePort, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);

            if (_clientSocket.Connected)
            {
                _clientSocket.EndConnect(result);
            }
            else
            {
                _clientSocket.Close();
                throw new SocketException(10060);
            }
            //_clientSocket.Connect(IPAddress.Parse(_remoteIp), _remotePort);
        }

        public void Send(ref List<byte> data)
        {
            byte[] sendData = data.ToArray();
            _clientSocket.Send(sendData);
        }

        public int Receive(out List<byte> data)
        {
            data = new List<byte>();
            byte[] recvData = new byte[2048];
            int recvLength = 0;
            recvLength = _clientSocket.Receive(recvData);

            if (recvLength > 0)
            {
                for (int i = 0; i < recvLength; i++)
                {
                    data.Add(recvData[i]);
                }

                return 0;
            }
            else
            {
                return -1;
            }
        }

        public void Close()
        {
            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Dispose();
            _clientSocket.Close();
        }
    }
}
