using System;
using System.Collections.Generic;
using System.Linq;
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
            _clinetSocket = new Socket(AddressFamily.InterNetwork, SocketType.Seqpacket, ProtocolType.Tcp);
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

        public int Init()
        {
            try
            {
                _clinetSocket.Bind(new IPEndPoint(IPAddress.Parse(_localIp), _localPort));
            }
            catch (FormatException)
            {
                return 1;
            }
            catch (ArgumentOutOfRangeException)
            {
                return 2;
            }
            catch (SocketException)
            {
                return 3;
            }
            catch (ObjectDisposedException)
            {
                return 4;
            }
            catch
            {
                return 5;
            }

            return 0;
        }

        public int Open()
        {
            try
            {
                _clinetSocket.Connect(IPAddress.Parse(_remoteIp), _remotePort);
            }
            catch (FormatException)
            {
                return 1;
            }
            catch (ArgumentOutOfRangeException)
            {
                return 2;
            }
            catch (SocketException)
            {
                return 3;
            }
            catch (ObjectDisposedException)
            {
                return 4;
            }
            catch (NotSupportedException)
            {
                return 5;
            }
            catch
            {
                return 6;
            }

            return 0;
        }

        public int Send(ref List<Byte> data)
        {
            byte[] sendData = data.ToArray();
            try
            {
                _clinetSocket.Send(sendData);
            }
            catch (SocketException)
            {
                return 1;
            }
            catch (ObjectDisposedException)
            {
                return 2;
            }
            catch
            {
                return 3;
            }

            return 0;
        }

        public int Receive(ref List<Byte> data)
        {
            List<ArraySegment<Byte>> recvData = new List<ArraySegment<byte>>();
            try
            {
                _clinetSocket.Receive(recvData);
            }
            catch (SocketException)
            {
                return 1;
            }
            catch (ObjectDisposedException)
            {
                return 2;
            }
            catch
            {
                return 3;
            }

            try
            {
                byte[] dataArray = recvData[0].Array;
                if (dataArray != null)
                {
                    data = dataArray.ToList();
                }
            }
            catch
            {
                return 5;
            }

            return 0;
        }

        public int Close()
        {
            try
            {
                _clinetSocket.Shutdown(SocketShutdown.Both);
                _clinetSocket.Dispose();
                _clinetSocket.Close();
            }
            catch (SocketException)
            {
                return 1;
            }
            catch (ObjectDisposedException)
            {
                return 2;
            }
            catch
            {
                return 3;
            }

            return 0;
        }
    }
}