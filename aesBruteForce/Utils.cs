using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace aesBruteForce
{
    class Utils
    {
        public static string BASE_PATH = @"C:\Users\Finkel\Desktop\Segurança\Desafio5\";

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

        public static void writeToFile(string filename, string[] lines)
        {
            System.IO.File.WriteAllLines(filename, lines);
        }

        public static byte[] concatenaBytes(byte[] a1, byte[] a2)
        {
            byte[] rv = new byte[a1.Length + a2.Length];
            Buffer.BlockCopy(a1, 0, rv, 0, a1.Length);
            Buffer.BlockCopy(a2, 0, rv, a1.Length, a2.Length);
            return rv;
        }

        public static string getPercentage(byte[] start, byte[] end, byte[] actual)
        {
            //NOT WORKING
            BigInteger startBig = new BigInteger(start);
            BigInteger endBig = new BigInteger(end);
            BigInteger actualBig = new BigInteger(actual);
            BigInteger d1 = BigInteger.Multiply(10000, (actualBig - startBig));
            BigInteger d2 = (endBig - startBig);
            BigInteger percentage = BigInteger.Divide(d1, d2);
            //Console.WriteLine("Start: " + startBig.ToString("X") + " End: " + endBig.ToString("X") + " Actual: " + actualBig.ToString("X") + " Percentage: " + percentage.ToString("X"));
            return percentage.ToString();
                                
        }

    }
}
