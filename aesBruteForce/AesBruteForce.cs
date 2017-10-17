using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
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
            Console.WriteLine("Version 0.1");
            //Tests.anwserTest();
            if (!Tests.executeAll())
            {
                Console.ReadKey();
                return;
            }


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Load encrypted text from file
            string[] encryptedTextLinesHex = Utils.readFromFile(Utils.BASE_PATH + "D5-GRUPO03.txt");
            int numberOfThreads = 8;


            byte[] keyConstant = Utils.asciiToByte("Key2Group03");

            byte[] attackBeginCode = Utils.hexToByte("2121212121",true);
            byte[] attackEndCode = Utils.hexToByte("7e7e7e7e7e",true);

            //Calcular intervalos de keys de cada thread
            BigInteger attackBeginCodeBig = new BigInteger(attackBeginCode);
            BigInteger attackEndCodeBig = new BigInteger(attackEndCode);
            BigInteger range = attackEndCodeBig - attackBeginCodeBig;
            BigInteger interval = BigInteger.Divide(range, numberOfThreads);

            Console.WriteLine("Range: " + range.ToString("X"));
            Console.WriteLine("Interval: " + interval.ToString("X"));

            for(int i = 0; i < numberOfThreads; ++i)
            {
                BigInteger beginBig = attackBeginCodeBig + BigInteger.Multiply(interval,i);
                BigInteger endBig = attackBeginCodeBig + BigInteger.Multiply(interval, i+1) -1;
                 
                if(i == (numberOfThreads - 1))
                {
                    endBig = attackEndCodeBig;
                }
                Console.WriteLine("Thread " + i + ": begin: " + beginBig.ToString("X") + " end: " + endBig.ToString("X"));

                Thread t = new Thread(() => threadTask(keyConstant, beginBig.ToByteArray(), endBig.ToByteArray(), encryptedTextLinesHex, stopwatch));
                t.Name = "" + i;
                t.Start();
            }
        }

        public static void threadTask(byte[] constantKeyPart, byte[] begin, byte[] end, string[] encryptedTextHexLines, Stopwatch stopwatch)
        {
            byte[] actualKey = Aes.getNextValidKey(begin);
            byte[] validBegin = actualKey.ToArray();
            byte[] validEnd = Aes.getNextValidKey(end);
            var lastMillis = stopwatch.ElapsedMilliseconds;
            byte[] encryptedFirstLine = Utils.hexToByte(encryptedTextHexLines[1],false);

            while (!actualKey.SequenceEqual(validEnd))
            {
                byte[] fullKey = Utils.concatenaBytes(constantKeyPart, actualKey);
                //Decrypt
                byte[] decrypted = Aes.decrypt(encryptedFirstLine, fullKey);

                //Check if it is portuguese
                if (mayBePortuguese(decrypted))
                {
                    string plainText = "";
                    foreach(var line in encryptedTextHexLines)
                    {
                        plainText +=  Utils.byteToAscii(Aes.decrypt(Utils.hexToByte(line,false), fullKey));
                    }
                    string[] lines = { "" + stopwatch.Elapsed, "Key: " + Utils.byteToAscii(fullKey), "Decrypted: " + plainText};
                    foreach(var line in lines){
                        Console.WriteLine(line);
                    }
                    Console.WriteLine("");
                    Utils.writeToFile(Utils.BASE_PATH + "possibleAnswer\\" + Utils.byteToHex(actualKey,true) + ".txt", lines);
                }
               
                //calculate next key
                actualKey = Aes.incrementKey(actualKey);
                //Console.WriteLine(Utils.byteToHex(actualKey,true));
                //Console.ReadKey();

                if (stopwatch.ElapsedMilliseconds - lastMillis > 60000)
                {
                    lastMillis = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine(stopwatch.Elapsed + " Thread " + Thread.CurrentThread.Name + ": " + Utils.getPercentage(validBegin, validEnd, actualKey)+ "(por mil) Start: " + new BigInteger(validBegin).ToString("X") + " End: " + new BigInteger(validEnd).ToString("X") + " Actual: " + new BigInteger(actualKey).ToString("X"));
                }
            }
        }

        private static bool mayBePortuguese(byte[] decrypted)
        {
            //verifica se só existem caracteres válidos para o português
            int bytesOutOfRange = 0;
            int counter = 0;
            while (bytesOutOfRange < 6 && counter < decrypted.Length)
            {
                if (decrypted[counter] < 97 || decrypted[counter] > 122 || decrypted[counter] == 32)
                {
                    bytesOutOfRange++;
                    //Console.WriteLine("Out of range");
                }
                counter++;
            }
            return bytesOutOfRange<5;
        }

    }
}
