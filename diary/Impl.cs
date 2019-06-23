using System;
namespace diary
{
    public class Impl
    {
        Cli cli;
        Diary diary;

        public Impl(Cli helper)
        {
            cli = helper;
        }
        public void Execute()
        {
            Console.WriteLine("Diary implementation");
            switch (cli.subcommand)
            {
                case "create":
                    Create();
                    break;
                case "report":
                    Report();
                    break;
                default:
                    Console.WriteLine($"Unrecognized command {cli.subcommand}");
                    break;
            }
        }
        public void Create()
        {
            Console.WriteLine("Create diary");
            diary = Diary.Create(Cli.filename, Cli.duration);
        }
        public void Report()
        {
            Console.WriteLine("Report generation");
            diary = Diary.Load(Cli.filename);
            diary.Report();
        }
    }
}
