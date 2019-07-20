using System.IO.Ports;

namespace Comm.Serial
{
    internal class TransportSerialAscii
    {
        public TransportSerialAscii(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
        {
            _baseSerial = new BaseSerial
            {
                PortName = portName,
                BaudRate = baudRate,
                DataBits = dataBits,
                Parity = parity,
                StopBits = stopBits
            };
        }

        private BaseSerial _baseSerial;

        public int Transport(ref string sendData, out string receiveData)
        {
            _baseSerial.Init();
            _baseSerial.Open();
            _baseSerial.SendString(ref sendData);
            _baseSerial.ReceiveString(out receiveData);
            _baseSerial.Close();
            return 0;
        }

        public void Close()
        {
            _baseSerial.Close();
        }
    }
}
