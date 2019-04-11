using System;
using System.IO;
using System.Text.RegularExpressions;

namespace search
{
    public class Impl
    {
        string dashes = new String('-', 64);
        Cli mycli;
        Regex candidateexp;

        private void ShowHeader(string filename)
        {

            Console.WriteLine(dashes);
            Console.WriteLine($"File {filename}");

            if (mycli.verbose)
            {
                Console.WriteLine(Path.GetFullPath(filename));
                DateTime creation = File.GetCreationTime(filename);
                DateTime modified = File.GetLastWriteTime(filename);
                FileInfo info = new System.IO.FileInfo(filename);

                Console.WriteLine("Created " + creation + "; modified " + modified);
                Console.WriteLine($"Size {info.Length}");
            }
            Console.WriteLine(dashes);

        }

        private void Search(string filename)
        {
            int linenum = 0;
            int occurrences = 0;
            System.IO.StreamReader file = null;
            try
            {
                file = new System.IO.StreamReader(filename);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error opening {filename}");
                Console.WriteLine(e.Message);
                return;
            }
            string line;
            while ((line = file.ReadLine()) != null)
            {
                linenum++;
                if (mycli.pattern)
                {
                    if (candidateexp.IsMatch(line))
                    {
                        occurrences++;
                        if (occurrences == 1)
                        {
                            ShowHeader(filename);
                        }
                        String linenumstr = linenum.ToString("D5");
                        Console.Write(linenumstr);
                        Console.WriteLine($": {line}");
                    }
                }
                else
                {
                    if (line.Contains(mycli.candidate))
                    {
                        occurrences++;
                        if (occurrences == 1)
                        {
                            ShowHeader(filename);
                        }
                        String linenumstr = linenum.ToString("D5");
                        Console.Write(linenumstr);
                        Console.WriteLine($": {line}");
                    }
                }

            }
            file.Close();

        }

        public Impl(Cli cli)
        {
            mycli = cli;
            if (mycli.verbose)
            {
                Console.WriteLine("Starting search");
            }
            if (mycli.pattern)
            {
                if (mycli.caseSensitive)
                {
                    candidateexp = new Regex(mycli.candidate, RegexOptions.Compiled);
                }
                else
                {
                    candidateexp = new Regex(mycli.candidate, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
            }
            foreach (string f in mycli.Arguments)
            {
                Search(f);
            }
        }
    }
}
