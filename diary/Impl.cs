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
            //Console.WriteLine("Diary implementation");
            filename = cli.filename;
            if (filename == null) filename = Cli.DEFAULT_DIARY_FILENAME;
        
            switch (cli.subcommand)
            {
                case "create":
                    Create();
                    break;
                case "report":
                    Report();
                    break;
                case "modify":
                    Modify();
                    break;
                case "close":
                    Close();
                    break;
                case "todo":
                    Todo();
                    break;
                case "done":
                    Done();
                    break;
                case "blocked":
                    Blocked();
                    break;
                default:
                    Console.WriteLine($"Unrecognized command {cli.subcommand}");
                    break;
            }
        }
        public void Create()
        {
            Console.WriteLine("Create diary");
            diary = Diary.Create(filename, cli.duration);
        }
        public void Report()
        {
            Console.WriteLine("Report generation");
            diary = Diary.Load(filename);
            diary.Report( cli.all );
        }
        public void Modify()
        {
            Console.WriteLine("Modify diary");
            diary = Diary.Load(filename);
            diary.duration = cli.duration;
            diary.Save(filename);
        }

        public void Close()
        {
            Console.WriteLine("Close current sprint");
            diary = Diary.Load(filename);
            diary.CloseSprint();
            diary.Save(filename);
        }

        public void Todo()
        {
            Console.WriteLine("Add a task to current sprint");
            diary = Diary.Load(filename);
            Todo todo = new Todo();
            todo.effort_estimate = cli.estimate;
            todo.project = cli.project;
            todo.description = cli.Operand();
            diary.active.Add(todo);
            diary.Save(filename);
        }

        public void Done()
        {
            Console.WriteLine("Mark a task Done");
            diary = Diary.Load(filename);
            if ((cli.task == null) ||  (cli.task.Length < 1))
            {
                Console.WriteLine("Need a task id to mark done");
                return;
            }
            Console.WriteLine($"Marking task {cli.task} done");
            diary.active.Done(cli.task);
            diary.Save(filename);
        }
        public void Blocked()
        {
            Console.WriteLine("Mark a task blocked");
            diary = Diary.Load(filename);
            if ((cli.task == null) || (cli.task.Length < 1))
            {
                Console.WriteLine("Need a task id to mark blocked");
                return;
            }
            Console.WriteLine($"Marking task {cli.task} blocked");
            diary.active.Blocked(cli.task,cli.Operand());
            diary.Save(filename);
        }
    }
}
