using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

            foreach (var line in plainTextLinesHex)
            {
                Console.Write(line+"\n");
            }
            Console.ReadLine();

            /*
            string plainText = "Texto para teste";
            string plainTextHex = Utils.toHex(plainText);
            string key = "essasenhaehfraca";
            string keyHex = Utils.toHex(key);
            string expectedHex = "A506A19333F306AC2C62CBE931963AE7 ";

            Console.WriteLine("Plain text: " + plainTextHex);
            Console.WriteLine("Key: " + keyHex);
            Console.ReadLine();

            Console.WriteLine("Decrypted : " + Aes.decrypt(Utils.HEX2Bytes("A506A19333F306AC2C62CBE931963AE7"), Encoding.ASCII.GetBytes(key)));
            Console.ReadLine();*/
        }
    }
}
