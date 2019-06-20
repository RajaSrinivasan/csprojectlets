using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace diary
{
    public class Diary
    {
        public int duration { get; set; }

        public Sprint current;                   // Current Sprint
        public List<Sprint> sprints;

        public Diary()
        {
            duration = 1;                        // Default duration is 1 week
            sprints = new List<Sprint>();        // Create an empty list of sprints
            current = Calendar.Create(duration); // Create a dummy current sprint
            sprints.Add(current);
        }

        public void Save(string filename)
        {
        }

        public static Diary Load(string filename)
        {
            return JsonConvert.DeserializeObject<Diary>(filename); //TODO
        }

        public void Show()
        {
            string json = JsonConvert.SerializeObject(this);
            Console.WriteLine(json);
        }

        public string Json()
        {
            sprints.Sort();
            return JsonConvert.SerializeObject(this);
        }

        public static Diary Create(string jsonstring)
        {
            Diary temp = JsonConvert.DeserializeObject<Diary>(jsonstring);
            temp.current = temp.sprints.Last();
            return temp;
        }

    }
}
