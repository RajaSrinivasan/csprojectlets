using System;
using Newtonsoft.Json;

namespace diary
{

    public class Todo
    {
        public enum StatusType
        {
            ToStart,
            InProgress,
            Done,
            Blocked
        }

        public string id { get; set; }
        public StatusType status { get; set; }

        public string description;
        public string blockage;
        public int effort_estimate { get; set; }
        public int effort_actual { get; set; }

        public Todo()
        {
        }

        public void Show()
        {
            Console.WriteLine($"ID={id}");
            string json = JsonConvert.SerializeObject(this);
            Console.WriteLine(json);
        }

        public string Json()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static Todo Create(string jsonstring)
        {
            return JsonConvert.DeserializeObject<Todo>(jsonstring);
        }
    }
}
