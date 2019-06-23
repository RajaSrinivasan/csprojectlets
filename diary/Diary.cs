using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace diary
{
    public class Diary
    {
        public DateTime created;
        public int duration { get; set; }            // Duration in weeks
        public List<Sprint> sprints;                 // Previous closed sprints
        public Sprint active;                        // Currently active sprint

        // Default Constructor
        //    No previous sprints
        //    Create a current sprint
        public Diary()
        {
            duration = 1;                        // Default duration is 1 week
            sprints = new List<Sprint>();        // Create an empty list of sprints
            active = Calendar.Create(duration);
            created = DateTime.Now ;
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

        // Create a diary file by
        //    - creating an empty diary
        //    - save it to the file
        //    filename - is the diary filename 
        //
        public static Diary Create(string filename,int sprdur)
        {
            
            string[] fields = filename.Split(Path.DirectorySeparatorChar);
            string homedir;
            string fullname = filename;
            if (fields.Length > 1)
            {
                if (fields[0] == "~")
                {
                    homedir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    fields[0] = homedir;
                    fullname = Path.Combine(fields);
                    Console.WriteLine($"Formed fullname {fullname}");
                }
            }

            if (File.Exists(fullname))
            {
                Console.WriteLine($"File {fullname} already exists. Not creating.");
                return null;
            }

            FileInfo fi = new FileInfo(fullname);
            Console.WriteLine($"In the directory {fi.DirectoryName}");
            Directory.CreateDirectory(fi.DirectoryName);

            Diary temp = new Diary();
            temp.duration = sprdur;
            Console.WriteLine($"Creating empty diary {fullname} duration {sprdur}");
            temp.Save(fullname);
            return temp;
        }

    }
}
