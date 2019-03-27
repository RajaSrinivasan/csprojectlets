using System;
using System.IO;

class Program
{
    static String name = "hello" ;
    static String version = "0.0.A" ;
    static String dashes = "----------------------------";
    static bool verbose = false;
    static void PrintFile(String filename)
    {
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

        Console.WriteLine(dashes);
        Console.WriteLine($"File {filename}");

        if (verbose)
        {
            Console.WriteLine(Path.GetFullPath(filename));
            DateTime creation = File.GetCreationTime(filename);
            DateTime modified = File.GetLastWriteTime(filename);
            FileInfo info = new System.IO.FileInfo(filename);

            Console.WriteLine("Created " + creation + "; modified " + modified);
            Console.WriteLine($"Size {info.Length}");
            Console.WriteLine(dashes);
        }
        int linenum = 0;
        string line;

        while ((line=file.ReadLine()) != null)
        {
            linenum++;
            String linenumstr = linenum.ToString("D5");
            //Console.Write(linenum.ToString("D" + (5 - linenumstr.Length).ToString() ) ) ;
            Console.Write(linenumstr);
            Console.WriteLine($": {line}");
        }
        file.Close();
    }

    static void ShowHelp()
    {
        Console.WriteLine($"{name} - {version}");
    }

    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            ShowHelp();
            return;
        }
        foreach (String arg in args)
        {
            switch (arg)
            {
                case "-h":
                    ShowHelp();
                    break;
                case "-v":
                    verbose = true;
                    break;
                default:
                    PrintFile(arg);
                    break;
            }
        }
    }
}

