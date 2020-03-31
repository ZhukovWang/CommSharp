using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Serial
{
    public class TransportSerialByte
    {
        public TransportSerialByte(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
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

        public int Transport(ref byte[] sendData, out byte[] receiveData)
        {
            _baseSerial.Init();
            _baseSerial.Open();
            _baseSerial.SendByte(ref sendData);
            _baseSerial.ReceiveByte(out receiveData);
            _baseSerial.Close();
            return 0;
        }

        public void Close()
        {
            _baseSerial.Close();
        }
    }
}