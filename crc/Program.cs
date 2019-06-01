using System;
using System.Security.Cryptography;

namespace crc
{
    class Program
    {
        /*static void TestBytes()
        {
            CRC crc = new CRC();

            byte b = 0;
            while (true)
            {
                ushort bytecrc;
                unsafe
                {
                    bytecrc = CRC.Checksum(&b, 1);
                }
                string bytecrcstr = bytecrc.ToString("X4");
                Console.WriteLine($"Byte={b} CRC={bytecrcstr}");
                if (b == 255) break;
                b++;
            }
        }

        static void TestWord()
        {
            CRC crc = new CRC();

            ushort w = 0;
            while (true)
            {
                ushort wordcrc;
                unsafe
                {
                    wordcrc = CRC.Checksum(&w, 2);
                }
                string wordcrcstr = wordcrc.ToString("X4");
                string wstr = w.ToString("D6");
                string whex = w.ToString("X4");
                if (w == wordcrc)
                {
                    Console.WriteLine($"Word={whex} CRC={wordcrcstr}");
                }
                if (w == 0xffff) break;
                w++;
            }
        }
        static void TestTable()
        {

            //CRC.Generate(0x8408);
            CRC.Generate(0x1021);   //CCITT
            //CRC.Generate(0xc0c1);
        }*/

        static void Main(string[] args)
        {
            Cli cli = new Cli(args);
            CRC crc = new CRC();
            if (cli.opt.Verbose)
            {
                crc.ShowTable();
            }
        }
    }
}
