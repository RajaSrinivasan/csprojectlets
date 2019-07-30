
// Copyright © 2019 TOPR llc.

// Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is 
// hereby granted, provided that the above copyright notice and this permission notice appear in all copies.

// THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS SOFTWARE
// INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS.IN NO EVENT SHALL THE AUTHOR BE LIABLE
// FOR ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
// LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION,
// ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.

// All questions may please be addressed to contact@toprllc.com


using System;
using CommandLine;

namespace ipadr
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Version v = new CommandLine.Version(0, 1, 1, "ipadr");

            Cli cli = new Cli();
            cli.usage_text = "ipaddr v4|v6 <adr> [<mask>]";
            cli.version = v;
            cli.AddSubcommand("v4", "IPV4");
            cli.AddSubcommand("v6", "IPV6");

            cli.AddSwitch("m", "mask", "test operand - is valid subnet mask?", null , "false" );

            cli.Analyze(args);
            if (cli.Present("help") || cli.Present("h")) cli.Help();
            if (cli.Present("m"))
            {
                Console.WriteLine("Mask validation");
                string mval = cli.GetString("m");
                Console.WriteLine("Mask value {0,0}", mval);
            }
            else
            {
                Console.WriteLine("No mask specified");
                if (cli.Present("mask"))
                {
                    Console.WriteLine("Mask value {0,0}", cli.GetString("mask"));
                }
            }
            foreach (string op in cli.Operands())
            {
                Console.WriteLine("Operand {0,1}", op);
            }

            string sc = cli.subcommand;
            IPAddress adr;
            switch (sc)
            {
                case "v4":
                    bool bstat = false;
                    adr = new IPV4Address();
                    string[] cliargs = cli.Operands();
                    if (cliargs.Length >= 1)
                    {
                        adr = new IPV4Address();
                        bstat = adr.Analyze(cliargs[0]);
                        if (!bstat)
                        {
                            Console.WriteLine("Please provide a valid IP Address");
                            return;
                        }
                        if (cliargs.Length >= 2)
                        {
                            bstat = adr.SetMask(cliargs[1]);
                            if (!bstat)
                            {
                                Console.WriteLine("Invalid network mask");
                                return;
                            }
                        }
                        else
                        {
                            if (cli.Present("m"))
                            {
                                string mval = cli.GetString("m");
                                adr = new IPV4Address();
                                bstat = adr.Analyze(cliargs[0]);
                                if (!bstat)
                                {
                                    Console.WriteLine("Invalid address for mask validation");
                                    return;
                                }
                                int mlen = IPV4Address.ValidMask((IPV4Address)adr);
                                if (mlen > 0)
                                {
                                    Console.WriteLine("Valid Mask");
                                    return;
                                }
                            }
                        }
                        adr.Show();
                    }
                    break;
                case "v6":
                    adr = new IPV6Address();
                    break;
                default:
                    Console.WriteLine("Unrecognized subcommand {0,1}", sc);
                    return;

            }
        }
    }
}
 