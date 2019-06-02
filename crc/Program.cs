using System;
using System.Security.Cryptography;

namespace crc
{
    class Program
    {
 

        static void Main(string[] args)
        {
            Cli cli = new Cli(args);
            CRC crc = new CRC();

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

            }
            else if (cli.opt.Lines)
            {

            }
            else if (cli.opt.HexString)
            {

            }
            else
            {
                unsafe
                {
                    byte[] bytarg = System.Text.Encoding.ASCII.GetBytes(cli.opt.Argument);

                    fixed (void* dptr = &bytarg[0])
                    {
                        ushort strcrc = crc.Checksum(dptr , bytarg.Length);
                        string strcrchex = strcrc.ToString("x4");
                        Console.WriteLine($"{strcrchex}");
                    }

                }
            }

        }
    }
}
