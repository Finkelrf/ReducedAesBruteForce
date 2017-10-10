using System;
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Load encrypted text from file
            string[] plainTextLinesHex = Utils.readFromFile(Utils.BASE_PATH + "D5-GRUPO03.txt");

            byte[] keyConstant = Encoding.ASCII.GetBytes("Key2Group03");

            byte[] begin1 = Utils.HEX2Bytes("2121212121");
            byte[] end1 = Utils.HEX2Bytes("3878787877");
            byte[] begin2 = Utils.HEX2Bytes("3878787878");
            byte[] end2 = Utils.HEX2Bytes("4fcfcfcfce");
            byte[] begin3 = Utils.HEX2Bytes("4fcfcfcfcf");
            byte[] end3 = Utils.HEX2Bytes("6727272726");
            byte[] begin4 = Utils.HEX2Bytes("6727272727");
            byte[] end4 = Utils.HEX2Bytes("7e7e7e7e7e");

            //byte[] begin1 = Utils.HEX2Bytes("2121212121");
            //byte[] end1 = Utils.HEX2Bytes("2121213021");
            //byte[] begin2 = Utils.HEX2Bytes("2121212131");
            //byte[] end2 = Utils.HEX2Bytes("2121212140");
            //byte[] begin3 = Utils.HEX2Bytes("2121212141");
            //byte[] end3 = Utils.HEX2Bytes("2121212150");
            //byte[] begin4 = Utils.HEX2Bytes("2121212151");
            //byte[] end4 = Utils.HEX2Bytes("2121212160");

            /*últimos 5 têm código ASCII entre 32 e 126 (decimal)*/

            //Calcular intervalos de keys de cada thread

            Thread t1 = new Thread(() => threadTask(keyConstant, begin1, end1, plainTextLinesHex, stopwatch));
            t1.Name = "Thread 1";
            t1.Start();
            Thread t2 = new Thread(() => threadTask(keyConstant, begin2, end2, plainTextLinesHex, stopwatch));
            t2.Name = "Thread 2";
            t2.Start();
            Thread t3 = new Thread(() => threadTask(keyConstant, begin3, end3, plainTextLinesHex, stopwatch));
            t3.Name = "Thread 3";
            t3.Start();
            Thread t4 = new Thread(() => threadTask(keyConstant, begin4, end4, plainTextLinesHex, stopwatch));
            t4.Name = "Thread 4";
            t4.Start();

        }

        //static void Main()
        //{
        //    // test main
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    System.Threading.Thread.Sleep(2000);
        //    Console.WriteLine(stopwatch.Elapsed);
        //    Console.ReadKey();

        //}

        public static void threadTask(byte[] constantKeyPart, byte[] begin, byte[] end, string[] plainTextHexLines, Stopwatch stopwatch)
        {
            byte[] actualKey = Aes.getNextValidKey(begin);
            byte[] validBegin = actualKey.ToArray();
            byte[] validEnd = Aes.getNextValidKey(end);
            var lastMillis = stopwatch.ElapsedMilliseconds;

            while (!actualKey.SequenceEqual(validEnd))
            {
                byte[] fullKey = Utils.concatenaBytes(constantKeyPart, actualKey);
                //Decrypt
                byte[] decrypted = Aes.decrypt(Encoding.ASCII.GetBytes(plainTextHexLines[0]), fullKey);

                //Check if it is portuguese
                if (mayBePortuguese(decrypted))
                {
                    string[] lines = { "Key: " + Utils.toHex(Encoding.ASCII.GetString(actualKey)), "Decrypted: " + Encoding.ASCII.GetString(decrypted), ""+ stopwatch.Elapsed};
                    foreach(var line in lines){
                        Console.WriteLine(line);
                    }
                    Console.WriteLine(Thread.CurrentThread.Name);
                    Console.WriteLine("");
                    Utils.writeToFile(Utils.BASE_PATH + "possibleAnswer\\" + Utils.toHex(Encoding.ASCII.GetString(actualKey)) + ".txt", lines);
                    //Console.ReadKey();
                }
                
                //Console.ReadKey();

                //calculate next key
                actualKey = Aes.incrementKey(actualKey);

                if(stopwatch.ElapsedMilliseconds - lastMillis > 60000)
                {
                    lastMillis = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine(stopwatch.Elapsed+" "+ Thread.CurrentThread.Name +": " +Utils.getPercentage(validBegin, validEnd, actualKey));
                }
            }
            Console.ReadKey();

        }

        private static bool mayBePortuguese(byte[] decrypted)
        {
            //verifica se só existem caracteres válidos para o português
            // se tiverem 5 ou mais characteres >137 ou  < 32
            int bytesOutOfRange = 0;
            int counter = 0;
            while (bytesOutOfRange < 5 && counter < decrypted.Length)
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
