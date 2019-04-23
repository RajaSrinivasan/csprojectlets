using System;
using System.Collections.Generic;
using System.Text;

namespace ppm
{
    public class Cli
    {
        private const string NAME = "ppm";
        private const int MAJOR_VERSION = 0;
        private const int MINOR_VERSION = 0;

        public bool verbose { set; get; } = false;

        public string Filename { set; get; } = "~/.ppm/password.dat";
        public string Command(string validcommands)
        {
            if (arguments.Count > 0)
            {
                if (validcommands.Contains(arguments[0]))
                {
                    return arguments[0];
                }

                throw new Exception("Unknwon command " + arguments[0]);
            }
            throw new Exception("No command specified");
        }

        public string Context 
        { 
            get
            {
                if (arguments.Count > 1)
                {
                    return arguments[1];
                }
                return "";
            } 
        }

        public string Username 
        { 
            get
            {
                if (arguments.Count > 2)
                {
                    return arguments[2];
                }
                return "";
            } 
        }

        public string Password 
        { 
            get
            {
                string ctx = Context;
                return GetPassword("Password for " + ctx );
            } 
        }

        private List<string> arguments = new List<string>();

        private void Help()
        {

        }

        private bool ProcessGlobalFlags(string[] args)
        {
            int argno = 0;
            while (true)
            {
                if (args.Length <= argno)
                {
                    break;
                }
                string arg = args[argno];
                switch (arg)
                {
                    case "-v":
                    case "--verbose":
                        verbose = true;
                        break;
                    case "-h":
                    case "--help":
                        Help();
                        return false;

                    case "-f":
                    case "--file":
                        argno++;
                        if (args.Length > argno)
                        {
                            Filename = args[argno];
                        }
                        break;
                    default:
                        if (arg.StartsWith("-"))
                        {
                            Console.WriteLine($"Unrecognized switch {arg}");
                            return false;
                        }
                        Console.WriteLine($"Adding {arg} to arguments");
                        arguments.Add(arg);
                        break;
                }
                argno++;

            }
            return true;
        }

        public string GetPassword(string prompt)
        {
            Console.Write(prompt);
            ConsoleKeyInfo cki;

            StringBuilder sb = new StringBuilder();

            while (true)
            {
                cki = Console.ReadKey(true);
                if (cki.KeyChar == '\n')
                {
                    break;
                }
                sb.Append(cki.KeyChar);
            }
            Console.WriteLine();
            return sb.ToString() ;
        }

        public Cli(string[] args)
        {
            if (args.Length < 1)
            {
                Help();
                return;
            }
            if (!ProcessGlobalFlags(args))
            {
                return;
            }
        }
    }
}
