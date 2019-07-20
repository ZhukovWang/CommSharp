using System;
using System.Collections.Generic;

namespace Comm
{
    internal class CheckSum
    {
        /// <summary>
        /// 计算CRC16校验，验证无问题
        /// </summary>
        /// <param name="data">验证的byte数组</param>
        /// <returns>返回的校验码，高位在1，地位在0</returns>
        public static byte[] Crc16(byte[] data)
        {
            int len = data.Length;

            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));

                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }

                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置
                return new[] { hi, lo };
            }

            return new byte[] { 0, 0 };
        }

        /// <summary>
        /// 计算CRC16校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ushort Crc16(ref List<byte> data)
        {
            ushort crcRes = 0xffff;
            foreach (var b in data)
            {
                crcRes ^= b;
                for (int i = 0; i < 8; i++)
                {
                    if ((crcRes & 0x01) != 0)
                    {
                        crcRes = (ushort)((crcRes >> 1) ^ 0xa001);
                    }
                    else
                    {
                        crcRes = (ushort)(crcRes >> 1);
                    }
                }
            }

            byte hi = (byte)((crcRes & 0xFF00) >> 8);  //高位置
            byte lo = (byte)(crcRes & 0x00FF);         //低位置
            crcRes = BitConverter.ToUInt16(new[] { hi, lo }, 0);

            return crcRes;
        }

        /// <summary>
        /// 计算LRC校验
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        public static byte Lrc8(ref List<byte> dataBytes)
        {
            byte lrcRes = 0;

            foreach (var b in dataBytes)
            {
                lrcRes = (byte)(((byte)(lrcRes + b)) & 0xFF);
            }
            lrcRes = (byte)(((lrcRes ^ 0xFF) + 1) & 0xFF);

            return lrcRes;
        }
    }
}
