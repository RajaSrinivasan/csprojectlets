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
                if (cli.opt.Argument.Length % 2 != 0)
                {
                    Console.WriteLine("Argument length is odd. Not a valid hex string");
                    return;
                }
                byte[] bytarg = new byte[cli.opt.Argument.Length / 2];
                for (int idx = 0; idx < bytarg.Length; idx++)
                {
                    string hexdig = cli.opt.Argument.Substring(idx * 2, 2);
                    try
                    {
                        byte b = Convert.ToByte(hexdig,16);
                        bytarg[idx] = b;
                    }
                    catch
                    {
                        Console.WriteLine($"Bad hex digit {hexdig}");
                        return;
                    }
                    
                }
                unsafe
                {
                    fixed (void* dptr = &bytarg[0])
                    {
                        ushort strcrc = crc.Checksum(dptr, bytarg.Length);
                        string strcrchex = strcrc.ToString("x4");
                        Console.WriteLine($"{strcrchex}");
                    }
                }
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
