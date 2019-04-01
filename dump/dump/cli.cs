using System;
using System.Collections.Generic;

namespace dump
{
    internal class Cli
    {
        private const int defaultblocksize = 32;
        public bool verbose;
        public bool octal = false;
        public int blocksize = defaultblocksize;
        public bool goodcli;

        private List<string> arguments = new List<string>() ;

        public Cli(string[] args)
        {
            if (args.Length < 1)
            {
                this.ShowHelp();
                return;
            }

            foreach (String arg in args)
            {   bool argisswitch;
                switch (arg)
                {
                    case "-v":
                    case "--verbose":
                        this.verbose = true;
                        break;
                    case "-h":
                    case "--help":
                        this.ShowHelp();
                        return;
                    case "-":
                    case "--octal":
                        this.octal = true;
                        break;
                    default:
                        argisswitch = false;
                        if (arg.StartsWith("-b:"))
                        {
                            argisswitch = true;
                            blocksize = int.Parse(arg.Substring(3));
                        }
                        if (arg.StartsWith("--blocksize:"))
                        {
                            argisswitch = true;
                            blocksize = int.Parse(arg.Substring(11));
                        }
                        if (argisswitch)
                        {
                    
                        }
                        else
                        {
                            if (arg.StartsWith("-"))
                            {
                                Console.WriteLine($"Unrecognized switch {arg}");
                                ShowHelp();
                                return;
                            }
                            this.arguments.Add(arg);
                        }
                        break;
                }
            }
            goodcli = true;
        }

        public List<string> Arguments { get => arguments; set => arguments = value; }

        public void ShowHelp()
        {
           Console.WriteLine("-h|--help       - show help");
           Console.WriteLine("-v|--verbose    - be verbose");
           Console.WriteLine($"-b|--blocksize  - blocksize. e.g. --blocksize:32 {defaultblocksize}");
        }
    }
   
}