using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace diary
{
    public class SprintCompare : IComparer<Sprint>
    {
        public int Compare(Sprint spr1, Sprint spr2)
        {
            return spr1.start.CompareTo(spr2.start);
        }
    }

    public class Sprint
    {
        public List<Todo> todos;

        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string id { get; set; }

        public Sprint()
        {
            todos = new List<Todo>();
            id = "notset";
        }

        public void Add(Todo todo)
        {
            todos.Add(todo);
            Console.WriteLine("Adding");
            todo.Show();
        }

        public void Show()
        {

            string json = JsonConvert.SerializeObject(this);
            Console.WriteLine(json);
            
            Console.WriteLine("------");
            Todo [] arrtodos = todos.ToArray();
            foreach (Todo todo in arrtodos)
            {
                Console.WriteLine("+++++");
                todo.Show();
            }
        }
        public string Json()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static Sprint Create(string jsonstring)
        {
            return JsonConvert.DeserializeObject<Sprint>(jsonstring);
        }

        public void Report()
        {
            Console.WriteLine($"Sprint ID : {id}");
            Console.WriteLine($"Starting {start.ToString()} Ending: {end.ToString()}");
            Console.WriteLine($"Tasks {todos.Count}");
        }
    }
}
