using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace aesBruteForce
{
    class Tests
    {
        public static bool executeAll()
        {
            if (!conversionTests())
                return false;
            if (!decryptionTest())
                return false;
            if (!keyIncrementTest())
                return false;
            return true;
        }

        public static bool conversionTests()
        {
            string hex = "5465737465";
            string ascii = "Teste";
            byte[] byteArray = { 0x54, 0x65, 0x73, 0x74, 0x65 };

            if (!Utils.asciiToHex(ascii).Equals(hex))
            {
                Console.WriteLine("asciiToHex failed");
                return false;
            }
            if (!Utils.asciiToByte(ascii).SequenceEqual(byteArray))
            {
                Console.WriteLine("asciiToByte failed");
                return false;
            }

            if (!Utils.byteToAscii(byteArray).Equals(ascii))
            {
                Console.WriteLine("byteToAscii failed expected: "+ascii+" got: "+ Utils.byteToAscii(byteArray));
                return false;
            }
            if (!Utils.byteToHex(byteArray, false).Equals(hex))
            {
                Console.WriteLine("byteToHex failed, expected: "+hex+" got: "+ Utils.byteToHex(byteArray, false));
                return false;
            }

            if (!Utils.hexToAscii(hex).Equals(ascii))
            {
                Console.WriteLine("hexToAscii failed");
                return false;
            }
            if (!Utils.hexToByte(hex, false).SequenceEqual(byteArray))
            {
                Console.WriteLine("hexToByte failed");
                return false;
            }
            if (!Utils.byteToHex(new byte[] { 0x11, 0x11, 0x11, 0x01, 0x11, 0x11 }, false).Equals("111111011111"))
            {
                Console.WriteLine("byteToHex with zeros failed");
                return false;
            }

            return true;
        }

        public static bool decryptionTest()
        {
            string plainText = "Texto para teste";
            string key = "essasenhaehfraca";
            string encryptedHex = "A506A19333F306AC2C62CBE931963AE7";

            if (!Utils.byteToAscii(Aes.decrypt(Utils.hexToByte(encryptedHex, false), Utils.asciiToByte(key))).Equals(plainText))
                return false;

            return true;
        }

        public static bool keyIncrementTest()
        {
            //if (!Aes.incrementKey(Utils.hexToByte("0000000000", true)).SequenceEqual(Utils.hexToByte("2121212121", true)))
            //{
            //    Console.WriteLine("Expected: 2121212121 Got: " + Utils.byteToHex(Aes.incrementKey(Utils.hexToByte("0000000000", true)),true));
            //    return false;
            //}
            if (!Aes.incrementKey(Utils.hexToByte("217e7e7e7e", true)).SequenceEqual(Utils.hexToByte("2221212121", true)))
            {
                Console.WriteLine("Expected: 2221212121 Got: " + Utils.byteToHex(Aes.incrementKey(Utils.hexToByte("217e7e7e7e", true)), true));
                return false;
            }

            if (!Aes.incrementKey(Utils.hexToByte("217e7e7e7e", true)).SequenceEqual(Utils.hexToByte("2221212121", true)))
            {
                Console.WriteLine("Expected: 2221212121 Got: " + Utils.byteToHex(Aes.incrementKey(Utils.hexToByte("217e7e7e7e", true)), true));
                return false;
            }
            return true;
        }

        public static bool anwserTest()
        {
            //byte b = 0x0a ;
            //Console.WriteLine(new BigInteger(b).ToString("X2"));

            //Console.ReadKey();

            byte[] keyConstant = Utils.asciiToByte("Key2Group03");
            byte[] keyVariable = Utils.hexToByte("6569625D57", false);
            byte[] fullKey = Utils.concatenaBytes(keyConstant, keyVariable);
            string[] encryptedTextLinesHex = Utils.readFromFile(Utils.BASE_PATH + "D5-GRUPO03.txt");

            Console.WriteLine("Key: " + Utils.byteToAscii(fullKey));
            Console.WriteLine("Encrypted:");
            foreach(var line in encryptedTextLinesHex)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("Decrypted:");
            foreach (var line in encryptedTextLinesHex)
            {
                byte[] decrypted = Aes.decrypt(fullKey, Utils.hexToByte(line, false));
                Console.WriteLine(Utils.byteToHex(decrypted,false));
            }
            Console.ReadKey();
            return true;
        }
    }
}
