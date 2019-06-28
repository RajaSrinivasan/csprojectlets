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
        public string project { get; set; }

        public Todo()
        {
            status = StatusType.ToStart;
        }

        public void Show()
        {
            Console.WriteLine($"ID={id}");
            string json = JsonConvert.SerializeObject(this);
            Console.WriteLine(json);
        }

        public void Report()
        {
            Console.WriteLine($"Todo: {id} Project: {project} Effort: {effort_estimate} Description: {description}");
            Console.WriteLine($"Status: {status}");
            Console.WriteLine($"Blockage: {blockage}");
            Console.WriteLine();
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
