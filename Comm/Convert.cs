using System;

namespace Comm
{
    public class Convert
    {
        public static byte ConvertBoolArrayToByte(bool[] source)
        {
            byte result = 0;
            // This assumes the array never contains more than 8 elements!
            int index = 8 - source.Length;

            // Loop through the array
            foreach (bool b in source)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }

        public static bool[] ConvertByteToBoolArray(byte b)
        {
            // prepare the return result
            bool[] result = new bool[8];

            // check each bit in the byte. if 1 set to true, if 0 set to false
            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;

            // reverse the array
            Array.Reverse(result);
            return result;
        }

        public static byte[] ConvertHexStringToBytes(string hexString)
        {
            if (hexString == null || hexString.Equals(""))
            {
                return null;
            }
            hexString = hexString.ToUpper();
            int length = hexString.Length / 2;
            char[] hexChars = hexString.ToCharArray();
            byte[] d = new byte[length];
            for (int i = 0; i < length; i++)
            {
                int pos = i * 2;
                d[i] = (byte)((byte)(hexChars[pos]) << 4 | (byte)(hexChars[pos + 1]));
            }
            return d;
        }

        public static string ConvertBytesToHexString(byte[] bytes) // 0xae00cf => "AE00CF "

        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                System.Text.StringBuilder strB = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
    }
}