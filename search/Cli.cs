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
        public string toplevel;
        public string candidate;

        private List<string> arguments = new List<string>() ;
        public List<string> Arguments { get => arguments; }

        private void ShowOptions()
        {
            Console.WriteLine($"Pattern={pattern} Case sensitive={caseSensitive} recursive={recursive} toplevel={toplevel}");
            Console.WriteLine($"Candidate={candidate}");
            foreach (string arg in arguments)
            {
                Console.WriteLine(arg);
            }
        }
        private void ShowHelpLine(string sw, string longsw, string help)
        {
            Console.WriteLine($"-{sw}\t| --{longsw} \t- {help}");
        }

        public void ShowHelp()
        {
            Console.WriteLine($"{NAME} - {MAJOR_VERSION}.{MINOR_VERSION}");
            Console.WriteLine("search [flags] <candidate> <target1> <target2> ...");
            ShowHelpLine("h", "help", "show this message");
            ShowHelpLine("v", "verbose", "be verbose");
            ShowHelpLine("c", "case", "case sensitive searches");
            ShowHelpLine("p", "pattern", "candidate is a pattern");

            ShowHelpLine("r", "recursive", "search all files and directories below the specified directory");
            ShowHelpLine("", "      ", "example: -r:. for current working directory");
        }

        private bool ExtractValue(string arg)
        {
            if (arg.StartsWith("-r:"))
            {
                toplevel = arg.Substring("-r:".Length );
                if (toplevel.Length >= 1) return true;
            }
            if (arg.StartsWith("--recursive:"))
            {
                toplevel = arg.Substring("--recursive:".Length);
                if (toplevel.Length >=1) return true;
            }

            Console.WriteLine($"Unrecognized switch {arg}");
            ShowHelp();
            return false;
        }

        public Cli(String []args)
        {
            if (args.Length < 2)
            {
                ShowHelp();

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


                    case "-p":
                    case "--pattern":
                        pattern = true;
                        break;

                    default:
                        if (arg.StartsWith("-r"))
                        {
                            if (!ExtractValue(arg))
                            {
                                return;
                            }
                            recursive = true;
                        }
                        else
                        {
                            if (candidate == null)
                            {
                                candidate = arg;
                            }
                            else
                            {
                                arguments.Add(arg);
                            }
                        }
                        break;

                }
            }

            if (verbose)
            {
                ShowOptions();
            }
        }
    }
}
