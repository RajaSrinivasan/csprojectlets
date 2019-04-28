using System;
using System.Collections.Generic;
using System.Text;

namespace ppm
{
    public class Cli
    {
        private const string NAME = "ppm";
        private const string DESCRIPTION = "Personal Password Manager";
        private const int MAJOR_VERSION = 0;
        private const int MINOR_VERSION = 0;
        public bool verbose = false;
        public bool copy_option = false;
        public string Filename { set; get; } = "password.dat";
        public string Command(string validcommands)
        {
            if (arguments.Count > 0)
            {
                if (validcommands.Contains(arguments[0]))
                {
                    return arguments[0];
                }

                Console.WriteLine($"Unknwon command {arguments[0]}"); ;
                return "";
            }
            return "";
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

        private void ShowHelpLine(string sw, string longsw, string help)
        {
            Console.WriteLine($"-{sw}\t| --{longsw} \t- {help}");
        }

        private void Help()
        {
            Console.WriteLine($"{NAME} - {DESCRIPTION} {MAJOR_VERSION}.{MINOR_VERSION}");
            Console.WriteLine("usage: ppm [flags] operation context username");

            Console.WriteLine("  Flags:");
            ShowHelpLine("v", "verbose", "be verbose");
            ShowHelpLine("h", "help", "print this message");
            ShowHelpLine("f", "file", "argument - password file.");

            Console.WriteLine("  Operations:");
            Console.WriteLine("create     - creates the password file if it does not exist");
            Console.WriteLine("add        - add a new context, username, password to the database");
            Console.WriteLine("update     - updates the password of the context, username");
            Console.WriteLine("show       - display the pawwsord of the context, username");
            Console.WriteLine("list       - lists all the entries in the database");
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
                    case "-c":
                    case "--copy":
                        copy_option = true;
                        break;
                    default:
                        if (arg.StartsWith("-"))
                        {
                            Console.WriteLine($"Unrecognized switch {arg}");
                            return false;
                        }
                        //Console.WriteLine($"Adding {arg} to arguments");
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
            string homedir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string ppmdir = System.IO.Path.Combine(homedir, ".ppm");
            if (!System.IO.Directory.Exists(ppmdir))
            {
                System.IO.Directory.CreateDirectory(ppmdir);
            }
            Filename = System.IO.Path.Combine(homedir, ".ppm", "password.dat");
            if (!ProcessGlobalFlags(args))
            {
                return;
            }
        }
       
    }
}
