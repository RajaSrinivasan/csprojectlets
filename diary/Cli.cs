using System;
using Utility.CommandLine;

namespace diary
{
    public class Cli
    {
        public string subcommand = "unknown";

        [Operands]
        private string[] Operands { get; set; }

        public string command ;
        public const string DEFAULT_DIARY_FILENAME = "~/.diary/diary.dia" ;        // in the home dir
        [Argument('f', "filename", "diary file name. default " + DEFAULT_DIARY_FILENAME)]
        public static string filename { get; set; } = DEFAULT_DIARY_FILENAME;

        [Argument('d', "duration", "duration of a sprint in weeks. Default=1")]
        public static int duration { get; set; } = 1;

        [Argument('e', "estimate","estimated effort for a task. (units agnostic)")]
        public static int estimate { get; set; }

        [Argument('p', "project" , "project name. default is blank")]
        public static string project { get; set; }  

        [Argument('t', "task" , "task identifier. Defautl - an auto generated id")]
        public static string task { get; set; }  

        [Argument('a', "all", "all tasks|sprints depending on context")]
        public static bool all { get; set; }


        [Argument('v', "verbose")]
        public static bool verbose { get; set; }

        [Argument('h', "help")]
        private static bool help { get; set; }

        private readonly string[]  commands = 
                                            {
                                                "create" ,   // create a new diary
                                                "report" ,   // report on the diary

                                                "close" ,    // close the current sprint and create a new one

                                                "todo" ,     // create a new task
                                                "modify" ,   // modify a task
                                                "remove" ,   // remove a task
                                                "done" ,     // mark a task done
                                                "blocked"    // mark a task as blocked

                                            };
        private void Help()
        {
            var helpAttributes = Arguments.GetArgumentInfo(typeof(Cli));

            foreach (var item in helpAttributes)
            {
                var result = item.ShortName + "\t" + String.Format("{0,-15}",item.LongName) + "\t" + item.HelpText;
                Console.WriteLine(result);
            }
        }
        public Cli(string[] args)
        {
            Arguments.Populate();

            if (args.Length >= 1)
            {
                if (help)
                {
                    Help();
                    return;
                }
            }
            else
            {
                Help();
                return;
            }
            Console.WriteLine($"Subcommad {args[0]}");
            if (Array.IndexOf(commands , args[0]) >= 0)
            {
                subcommand = args[0];
            }
            Console.WriteLine($"Duration {duration}");
            if (Operands != null)
            {
                foreach (string operand in Operands)
                {
                    Console.WriteLine("\r\n Operand:" + operand);
                }
            }
        }
    }
}
