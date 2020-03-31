using System;
using System.Collections.Generic;

namespace Comm.Socket
{
    public class TransportTcpSocket
    {
        public TransportTcpSocket(string remoteIp, int remotePort, string localIp, int localPort)
        {
            _baseSocket = new BaseSocket(remoteIp, remotePort, localIp, localPort);
        }

        private BaseSocket _baseSocket;

        public void TransportOpen()
        {
            _baseSocket.Init();
            _baseSocket.Open();
        }

        public int Transport(ref List<Byte> sendData, out List<Byte> receiveData)
        {
            receiveData = new List<byte>();
            _baseSocket.Send(ref sendData);
            int error = _baseSocket.Receive(out receiveData);
            return error;
        }

        public void TransportClose()
        {
            _baseSocket.Close();
        }
    }

    internal class TransportUdpSocket
    {
        public TransportUdpSocket(string remoteIp, int remotePort, string localIp, int localPort)
        {
            _udpSocket = new UdpSocket(remoteIp, remotePort, localIp, localPort);
        }

        private UdpSocket _udpSocket;
        private bool _timeToClose = false;

        public int TransportUdpSocketInit()
        {
            int error = _udpSocket.Init();
            return error;
        }

        public int TransportUdpSocketData(ref List<Byte> sendData, out List<Byte> receiveData)
        {
            receiveData = new List<byte>();
            List<byte> receiveOneTimeData = new List<byte>();
            int error = _udpSocket.Send(ref sendData);

            if (error == 0)
            {
                LaunchTime();

                while (true)
                {
                    if (_timeToClose)
                    {
                        break;
                    }

                    error = _udpSocket.Receive(out receiveOneTimeData);

                    if (error == 0)
                    {
                        foreach (byte b in receiveOneTimeData)
                        {
                            receiveData.Add(b);
                        }

                        receiveOneTimeData = new List<byte>();
                    }

                    error = 0;
                }
            }

            return error;
        }

        private void LaunchTime()
        {
            System.Timers.Timer t = new System.Timers.Timer(1000);//实例化Timer类，设置间隔时间为10000毫秒；
            t.Elapsed += new System.Timers.ElapsedEventHandler(CloseUdp);//到达时间的时候执行事件；
            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void CloseUdp(object source, System.Timers.ElapsedEventArgs e)
        {
            _timeToClose = true;
        }
    }
}