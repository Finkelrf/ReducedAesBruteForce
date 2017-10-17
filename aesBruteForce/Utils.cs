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

        public static string asciiToHex(string input)
        {
            char[] charValues = input.ToCharArray();
            string hexOutput = "";
            foreach (char _eachChar in charValues)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(_eachChar);
                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput += String.Format("{0:x2}", value);
                // to make output as your eg 
                //  hexOutput +=" "+ String.Format("{0:X}", value
            }

            return hexOutput;
        }

        public static byte[] asciiToByte(string input)
        {
            return hexToByte(asciiToHex(input), false);
        }

        public static byte[] hexToByte(string hex, bool inverted)
        {
            byte[] b = BigInteger.Parse(hex, NumberStyles.AllowHexSpecifier).ToByteArray();
            //if (b.Length<5)
            //{
            //    int missingBytes = 5 - b.Length;
            //    byte[] newArray = new byte[5];
            //    b.CopyTo(newArray, 0);
            //    b = newArray;
            //}

            //default is to reverse the byte order
            if (!inverted)
            {
                b = b.Reverse().ToArray();
            }
            return b;
        }

        public static string hexToAscii(string hex)
        {
            return byteToAscii(hexToByte(hex, false));
        }

        public static string byteToAscii(byte[] hexByteArray)
        {
            string hexString = byteToHex(hexByteArray,false);

            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }

        public static string byteToHex(byte[] bytes, bool inverted)
        {
            string hexString = "";
            if (inverted)
            {
                bytes = bytes.Reverse().ToArray();
            }
            foreach(var b in bytes)
            {
                hexString += b.ToString("X2");
            }
            return hexString;
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
            BigInteger startBig = new BigInteger(start);
            BigInteger endBig = new BigInteger(end);
            BigInteger actualBig = new BigInteger(actual);
            BigInteger d1 = BigInteger.Multiply(1000, (actualBig - startBig));
            BigInteger d2 = (endBig - startBig);
            BigInteger percentage = BigInteger.Divide(d1, d2);
            //Console.WriteLine("Start: " + startBig.ToString("X") + " End: " + endBig.ToString("X") + " Actual: " + actualBig.ToString("X") + " Percentage: " + percentage.ToString("X"));
            return percentage.ToString();

        }

    }
}
