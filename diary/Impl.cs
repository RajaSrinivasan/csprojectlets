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
        private string filename;
        public void Execute()
        {
            Console.WriteLine("Diary implementation");
            filename = Cli.filename;
            if (filename == null) filename = Cli.DEFAULT_DIARY_FILENAME;
        
            switch (cli.subcommand)
            {
                case "create":
                    Create();
                    break;
                case "report":
                    Report();
                    break;
                case "close":
                    Close();
                    break;
                default:
                    Console.WriteLine($"Unrecognized command {cli.subcommand}");
                    break;
            }
        }
        public void Create()
        {
            Console.WriteLine("Create diary");
            diary = Diary.Create(filename, Cli.duration);
        }
        public void Report()
        {
            Console.WriteLine("Report generation");
            diary = Diary.Load(filename);
            diary.Report();
        }
        public void Close()
        {
            Console.WriteLine("Close current sprint");
            diary = Diary.Load(filename);
            diary.CloseSprint();
            diary.Save(filename);
        }
    }
}
