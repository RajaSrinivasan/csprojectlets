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

    public class Sprint : IComparable<Sprint>
    {
        public enum StatusType
        {
            CLOSED ,
            OPEN
        }
        public List<Todo> todos;

        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string id { get; set; }
        public StatusType status { get; set; }
        public int CompareTo(Sprint other)
        {
            return string.Compare(this.id, other.id);
        }
        public Sprint()
        {
            todos = new List<Todo>();
            id = "notset";
        }

        public void AssignID(Todo todo)
        {
            todo.id = id + ":" + "T" + todos.Count.ToString();
        }

        public void Add(Todo todo)
        {
            AssignID(todo);
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

        public int TotalEffortEstimate()
        {
            int totalestimate = 0;
            foreach (Todo todo in todos)
            {
                totalestimate += todo.effort_estimate;
            }
            return totalestimate;
        }
        public void Done(string todoid)
        {
            foreach (Todo todo in todos)
            {
                if (todoid.Equals(todo.id))
                {
                    todo.status = Todo.StatusType.Done;
                    return;
                }
            }
            Console.WriteLine($"No such task {todoid} to mark done");
        }
        public void Blocked(string todoid , string blockage)
        {
            foreach (Todo todo in todos)
            {
                if (todoid.Equals(todo.id))
                {
                    todo.status = Todo.StatusType.Blocked;
                    if (blockage.Length < 1) todo.blockage = "unknown";
                    else todo.blockage = blockage;
                    return;
                }
            }
            Console.WriteLine($"No such task {todoid} to mark blocked");
        }

        public void Report()
        {
            DateTime now = DateTime.Now;

            Console.WriteLine($"Sprint ID : {id} Status: {status} -----------------------");
            if (now > end)
            {
                Console.WriteLine("State: Expired. Should be closed");
            }
            else
            {
                Console.WriteLine("State: Active");
            }
            Console.WriteLine($"Starting {start.ToString()} Ending: {end.ToString()}");
            Console.WriteLine($"Tasks {todos.Count} Total Effort: {TotalEffortEstimate()}");
            foreach (Todo todo in todos)
            {
                todo.Report();
            }
        }
    }
}
