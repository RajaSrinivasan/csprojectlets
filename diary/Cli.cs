using System;
using System.Collections.Generic;
namespace diary
{
    public class Cli
    {
        public string subcommand = "unknown";
      
        public const string DEFAULT_DIARY_FILENAME = "~/.diary/diary.dia" ;        // in the home dir

        // -f --filename
        public  string filename { get; set; } = DEFAULT_DIARY_FILENAME;

        // -d --duration
        public  int duration { get; set; } = 1;

        // -e --estimate
        public  int estimate { get; set; }
        // -p --project
        public  string project { get; set; }  
        // -t --task
        public  string task { get; set; }  
        // -a --all
        public  bool all { get; set; }
     
        // -v --verbose
        public  bool verbose { get; set; }
        // -h --help
        private  bool help { get; set; }

        private readonly string[]  commands = 
                                            {
                                                "create" ,   // create a new diary
                                                "report" ,   // report on the diary
                                                "modify" ,   // modify the parameters of the diary (sprint duration e.g.)
                                                "close" ,    // close the current sprint and create a new one

                                                "todo" ,     // create a new task
                                                "remove" ,   // remove a task
                                                "done" ,     // mark a task done
                                                "blocked"    // mark a task as blocked

                                            };
        private int lastswitch = -1;
        private List<string> operands = new List<string>();
        private void Help()
        {

        }
        public string[] Operands()
        {
            return operands.ToArray();
        }
        public string Operand()
        {
            return string.Join(" ",operands);
        }
        public Cli(string[] args)
        {

            if (args.Length == 0)
            {
                Help();
                return;
            }

            if (Array.IndexOf(commands , args[0]) >= 0)
            {
                subcommand = args[0];
            }
            else
            {
                Console.WriteLine($"Unrecognized command {args[0]}");
                return;
            }

            int arg = 1;
            while  (arg < args.Length)
            {
                
                switch (args[arg])
                {
                    case "-h":
                    case "--help":
                        Help();
                        lastswitch = arg;
                        return;
                    case "-v":
                    case "--verbose":
                        verbose = true;
                        lastswitch = arg;
                        break;

                    case "-a":
                    case "--all":
                        all = true;
                        lastswitch = arg;
                        break;

                    case "-d":
                    case "--duration":
                        if (arg+1 < args.Length)
                        {
                            duration = int.Parse(args[arg + 1]);
                            arg++;
                        }
                        lastswitch = arg;
                        break;
                    case "-e":
                    case "--estimate":
                        if (arg+1 < args.Length)
                        {
                            estimate = int.Parse(args[arg + 1]);
                            arg++;
                        }
                        lastswitch = arg;
                        break;
                    case "-p":
                    case "--project":
                        if (arg+1 < args.Length)
                        {
                            project = args[arg + 1];
                            arg++;
                        }
                        lastswitch = arg;
                        break;
                    case "-t":
                    case "--task":
                        if (arg+1 < args.Length)
                        {
                            task = args[arg + 1];
                            arg++;
                        }
                        lastswitch = arg;
                        break;
                    default:
                        break;

                }
                arg++;
            }

            for (int i=lastswitch+1; i<args.Length; i++)
            {
                operands.Add(args[i]);
            }
        }

    }
}
