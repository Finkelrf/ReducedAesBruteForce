using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*the fifth cryptographic challenge you should implement a reduced brute force attack against a text encrypted with the advanced encryption algorithm 
or aes operating in ecbmode or electronic code book mode but without the knowledge of the corresponding plain text and you should be able to analyze
a string of bytes and find a heuristic method to distinguish between random gibberish and a coherent message written in portuguese

AES ECB mode*/

namespace aesBruteForce
{
    class AesBruteForce
    {
        static void Main(string[] args)
        {
            //Load encrypted text from file
            string PATH = @"C:\Users\Finkel\Desktop\Segurança\Desafio5\D5-GRUPO03.txt";
            string[] plainTextLinesHex = Utils.readFromFile(PATH);

            byte[] keyConstant = Encoding.ASCII.GetBytes("Key2Group03");
            byte[] begin = Utils.HEX2Bytes("2121212121");
            byte[] end = Utils.HEX2Bytes("7e7e7e7e7e");
            int numberOfThreads = 8;

            /*últimos 5 têm código ASCII entre 33 e 126 (decimal)*/

            // Calcular intervalos de keys de cada thread

            Thread t1 = new Thread(() => threadTask(keyConstant, begin, end, plainTextLinesHex));
            t1.Name = "Secundária - ";
            t1.Start();

        }

        //static void Main()
        //{
        //    // test main
        //    byte[] b1 = Encoding.ASCII.GetBytes("Key2Group03");
        //    byte[] b2 = Utils.HEX2Bytes("2121212121");
        //    Console.WriteLine(Encoding.ASCII.GetString(concatenaBytes(b1,b2)));
        //    Console.ReadKey();

        //}
        public static void threadTask(byte[] constantKeyPart, byte[] begin, byte[] end, string[] plainTextHexLines)
        {
            byte[] actualKey = Aes.getNextValidKey(begin);
            byte[] validEnd = Aes.getNextValidKey(end);

            while (actualKey != validEnd)
            {
                byte[] fullKey = Utils.concatenaBytes(constantKeyPart, actualKey);
                //Decrypt
                byte[] decrypted = Aes.decrypt(Encoding.ASCII.GetBytes(plainTextHexLines[0]), fullKey);

                //Check if it is portuguese
                if (mayBePortuguese(decrypted))
                {
                    Console.WriteLine("Key: " + Utils.toHex(Encoding.ASCII.GetString(actualKey)));
                    Console.WriteLine("Decrypted: " + decrypted);
                    Console.WriteLine("");
                    Console.ReadKey();
                }

                //calculate next key
                actualKey = Aes.incrementKey(actualKey);
            }

        }

        private static bool mayBePortuguese(byte[] decrypted)
        {
            return true;
        }

    }
}
