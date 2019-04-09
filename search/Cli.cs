using System;
using System.Collections.Generic;

namespace search
{
    public class Cli
    {
        private const string NAME = "search";
        private const int MAJOR_VERSION = 0;
        private const int MINOR_VERSION = 0;

        public bool verbose = false;
        public bool pattern = false;
        public bool caseSensitive = false;
        public bool recursive = false;


        private List<string> arguments = new List<string>() ;
        public List<string> Arguments { get => arguments; }

        private void ShowHelpLine(string sw, string longsw, string help)
        {
            Console.WriteLine($"-{sw} | --{longsw} \t- {help}");
        }

        public void ShowHelp()
        {
            Console.WriteLine($"{NAME} - {MAJOR_VERSION}.{MINOR_VERSION}");
        }

        public Cli(String []args)
        {
            if (args.Length < 2)
            {
                ShowHelp();
                ShowHelpLine("h", "help", "show this message");
                ShowHelpLine("v", "verbose", "be verbose");
                ShowHelpLine("c", "case", "case sensitive searches");
                return;
            }

            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "-h":
                    case "--help":
                        ShowHelp();
                        return;

                    case "-v":
                    case "--verbose":
                        verbose = true;
                        break;

                    case "-c":
                    case "--case":
                        caseSensitive = true;
                        break;

                    case "-r":
                    case "--recursive":
                        recursive = true;
                        break;

                    case "-p":
                    case "--pattern":
                        pattern = true;
                        break;

                    default:
                        if (arg.StartsWith("-"))
                        {
                            Console.WriteLine($"Unrecognized switch {arg}");
                            ShowHelp();
                            return;
                        }
                        this.arguments.Add(arg);
                        break;

                }
            }
        }
    }
}
