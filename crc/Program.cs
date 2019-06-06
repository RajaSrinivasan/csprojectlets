using System;
using System.IO;
using System.Security.Cryptography;

namespace crc
{
    class Program
    {
        static void ShowHex(ushort val)
        {
            string hexval = val.ToString("x4");
            Console.WriteLine(hexval);
        }
        static void FileArg(CRC crc,Cli cli)
        {
            ushort csum = crc.Checksum(cli.opt.Argument);
            ShowHex(csum);
        }

        static void LinesArg(CRC crc,Cli cli)
        {
            System.IO.StreamReader file = null;
            string filename = cli.opt.Argument;
            try
            {
                file = new System.IO.StreamReader(filename);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error opening {filename}");
                Console.WriteLine(e.Message);
                return;
            }

            string line;
            int linenum = 0;
            ushort linecrc;
            while ((line = file.ReadLine()) != null)
            {
                linenum++;
                if (cli.opt.HexString)
                {
                    linecrc = crc.HexStringChecksum(line);
                }
                else
                {
                    linecrc = crc.Checksum(line);
                }
                Console.Write($"{linenum} : ");
                ShowHex(linecrc);
            }
            file.Close();

        }

        static void HexStringArg(CRC crc, Cli cli)
        {
            ushort result = crc.HexStringChecksum(cli.opt.Argument);
            ShowHex(result);
        }

        static void StringArg(CRC crc, Cli cli)
        {
            ushort result = crc.Checksum(cli.opt.Argument);
            ShowHex(result);
        }

        static void Main(string[] args)
        {
            Cli cli;
            try
            {
                cli = new Cli(args);
            }
            catch
            {
                Console.WriteLine("Command Line error");
                return;
            }

            if (cli.opt == null)
            {
                return;
            }

            CRC crc;
            if (cli.opt.Noise > 0)
            {
                crc = new NoiseCRC(cli.opt.Noise) ;
                if (cli.opt.Verbose)
                {
                    Console.WriteLine($"Enabling noise level {cli.opt.Noise}");
                }
            }
            else
            {
                crc = new CRC();
            }

            if ((cli.opt.Polynomial != null) && (cli.opt.Polynomial.Length > 0))
            {
                try
                {
                    ushort poly = System.Convert.ToUInt16(cli.opt.Polynomial , 16);
                    crc.Generate(poly);
                }
                catch
                {
                    Console.WriteLine($"Invalid polynomial spec {cli.opt.Polynomial}");
                    return;
                }
            }

            if (cli.opt.Table)
            {
                crc.ShowTable();
                return;
            }

            if (cli.opt.Argument == null)
            {
                return;
            }

            if (cli.opt.File)
            {
                FileArg(crc, cli);
                return;
            }
            else if (cli.opt.Lines)
            {
                LinesArg(crc, cli);
                return;
            }
            else if (cli.opt.HexString)
            {
                HexStringArg(crc, cli);
                return;
            }
            else
            {
                StringArg(crc, cli);
            }

        }
    }
}
