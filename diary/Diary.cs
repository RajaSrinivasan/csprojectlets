using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace diary
{
    public class Diary
    {
        public int duration { get; set; }

        public List<Sprint> sprints;

        public Diary()
        {
            duration = 1;                        // Default duration is 1 week
            sprints = new List<Sprint>();        // Create an empty list of sprints
            Sprint current; // Create a dummy current sprint
            current = Calendar.Create(duration);
            sprints.Add(current);
        }

        public void Save(string filename)
        {
            System.IO.StreamWriter file = null;
            try
            {
                file = new System.IO.StreamWriter(filename);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error creating {filename}");
                Console.WriteLine(e.Message);
                return;
            }
            string js = Json();
            file.WriteLine(js);
            file.Close();

        }

        public static Diary Load(string filename)
        {
            System.IO.StreamReader file = null;
            try
            {
                file = new System.IO.StreamReader(filename);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error reading {filename}");
                Console.WriteLine(e.Message);
                return null;
            }
            string js = file.ReadLine();
            file.Close();
            return JsonConvert.DeserializeObject<Diary>(js);
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
            return temp;
        }

    }
}
