using System.IO.Ports;

namespace Comm.Serial
{
    internal class BaseSerial
    {
        public BaseSerial()
        {
            _serialPort = new SerialPort();
        }

        private string _portName;

        public string PortName
        {
            get { return _portName; }
            set { _portName = value; }
        }

        private int _baudRate;

        public int BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        private Parity _parity;

        public Parity Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        private int _dataBits;

        public int DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        private StopBits _stopBits;

        public StopBits StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        private Handshake _handshake = Handshake.None;

        private SerialPort _serialPort;

        public void Init()
        {
            _serialPort.PortName = _portName;
            _serialPort.BaudRate = _baudRate;
            _serialPort.Parity = _parity;
            _serialPort.DataBits = _dataBits;
            _serialPort.StopBits = _stopBits;
            _serialPort.Handshake = _handshake;
            _serialPort.NewLine = "\r\n";
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
        }

        public void Open()
        {
            _serialPort.Open();
        }

        public void SendString(ref string data)
        {
            _serialPort.WriteLine(data);
        }

        public void ReceiveString(out string data)
        {
            data = _serialPort.ReadLine();
        }

        public void SendByte(ref byte[] data)
        {
            _serialPort.Write(data, 0, data.Length);
        }

        public void ReceiveByte(out byte[] data)
        {
            data = new byte[1024];
            _serialPort.Read(data, 0, 1024);
        }

        public void Close()
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }
    }
}
