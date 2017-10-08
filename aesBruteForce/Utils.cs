using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aesBruteForce
{
    class Utils
    {
        public static string toHex(string input)
        {
            return string.Join(string.Empty, input.Select(c => ((int)c).ToString("X")).ToArray());
        }

        public static byte[] HEX2Bytes(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture,
                                                          "The binary key cannot have an odd number of digits: {0}", hex));
            }

            byte[] HexAsBytes = new byte[hex.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hex.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }

        public static string[] readFromFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            return lines;
        }
    }
}
