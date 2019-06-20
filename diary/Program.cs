using System;

namespace diary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Todo todo = new Todo();
            todo.id = "No.1";
            todo.status = Todo.StatusType.ToStart;
            todo.description = "Get Started";
            todo.blockage = "None";
            todo.Show();
            Console.WriteLine("----");

            string strtodo = todo.Json();
            Todo newtodo = Todo.Create(strtodo);
            Console.WriteLine("Deserialized Todo");
            newtodo.Show();
            Console.WriteLine("----");
            Sprint sprint = new Sprint();
            for (int i=0; i<5; i++)
            {
                Todo todonew = new Todo();
                //todonew = todo;
                todonew.id = i.ToString();
                sprint.Add(todonew);
            }
            sprint.Show();

            Sprint newsprint = Calendar.Create(1);
            newsprint.Show();

            Sprint longsprint = Calendar.Create(3);
            longsprint.Show();

            Diary diary = new Diary();
            
        }
    }
}
