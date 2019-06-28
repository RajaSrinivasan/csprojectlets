using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace diary
{
    public class Diary
    {
        public static int DEFAULT_DURATION = 1;
        public DateTime created;
        public int duration { get; set; }            // Duration in weeks
        public List<Sprint> sprints;                 // Previous closed sprints
        public Sprint active;                        // Currently active sprint

        // Default Constructor
        //    No previous sprints
        //    Create a current sprint
        private Diary()
        {
        }

        private void AssignID(Sprint spr)
        {
            spr.id = "S" + sprints.Count.ToString("D");
        }
        private static string Fullname( string filename)
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
                    //Console.WriteLine($"Formed fullname {fullname}");
                }
            }

            FileInfo fi = new FileInfo(fullname);
            //Console.WriteLine($"In the directory {fi.DirectoryName}");
            Directory.CreateDirectory(fi.DirectoryName);
            return fullname;
        }

        public void Save(string filename)
        {
            string fullname = Fullname(filename);
            System.IO.StreamWriter file = null;
            try
            {
                file = new System.IO.StreamWriter(fullname);
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

            //Console.WriteLine($"Created {filename}");
            //Console.WriteLine(js);

        }

        public static Diary Load(string filename)
        {
            string fullname = Fullname(filename);
            System.IO.StreamReader file = null;
            try
            {
                file = new System.IO.StreamReader(fullname);
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
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        // Create a diary file by
        //    - creating an empty diary
        //    - save it to the file
        //    filename - is the diary filename 
        //
        public static Diary Create(string filename,int sprdur)
        {
            string fullname = Fullname(filename);

            if (File.Exists(fullname))
            {
                Console.WriteLine($"File {fullname} already exists. Not creating.");
                return null;
            }

            Diary temp = new Diary();
            if (sprdur != 0) temp.duration = sprdur; // Default duration is 1 week
            else temp.duration = Diary.DEFAULT_DURATION;

            temp.sprints = new List<Sprint>();       // Create an empty list of sprints
            
            temp.active = Calendar.Create(temp.duration);
            temp.AssignID(temp.active);
            temp.created = DateTime.Now;

            Console.WriteLine($"Creating empty diary {fullname} duration {sprdur}");
            temp.Save(fullname);

            return temp;
        }

        public void Report(bool all)
        {
            string cr = created.ToString();
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine($"Created : {cr}");
            Console.WriteLine($"Sprint Duration : {duration}");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Current Sprint:");
            active.Report();
            Console.WriteLine("---------------------------------------------------------");

            if (all)
            {
                if (sprints.Count == 0)
                {
                    Console.WriteLine("No previous sprints");
                }
                else
                {
                    foreach (Sprint spr in sprints)
                    {
                        spr.Report();
                    }
                }
            }
        }

        public void CloseSprint()
        {
            DateTime now = DateTime.Now;
            active.status = Sprint.StatusType.CLOSED;
            if (now < active.end)
            {
                Console.WriteLine("Warning: Sprint expiry date is away. Closing");
            }
            if (sprints == null)
            {
                Console.WriteLine("No previous sprints. Creating a list");
                sprints = new List<Sprint>();
            }

            sprints.Add(active);
            active = Calendar.Create(duration);
            AssignID(active);
        }

       
    }
}
