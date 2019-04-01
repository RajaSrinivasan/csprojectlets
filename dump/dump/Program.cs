using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dump
{
    class Program
    {
        static string dashes = "-------------------------------------------------------------";
        static void DumpFile(string filename, Cli cli)
        {
            System.IO.BinaryReader file = null;
            try
            {
                file = new System.IO.BinaryReader(File.Open(filename, FileMode.Open) );
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error opening {filename}");
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine(dashes);
            Console.WriteLine($"File {filename}");

            Console.WriteLine(Path.GetFullPath(filename));
            DateTime creation = File.GetCreationTime(filename);
            DateTime modified = File.GetLastWriteTime(filename);
            FileInfo info = new System.IO.FileInfo(filename);

            Console.WriteLine("Created " + creation + "; modified " + modified);
            Console.WriteLine($"Size {info.Length}");
            Console.WriteLine(dashes);
            byte[] buffer;
            int offset = 0;
            while (true)
            {
                buffer = file.ReadBytes(cli.blocksize);
                if (buffer.Length == 0)
                {
                    break;
                }
                char []bufchars = System.Text.Encoding.ASCII.GetString(buffer).ToCharArray();
                Console.Write(offset.ToString("D8"));
                Console.Write(" : ");
                foreach (char c in bufchars)
                {
                    if (Char.IsControl(c))
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write(c);
                    }
                }
                if (bufchars.Length < cli.blocksize)
                {
                    Console.Write(new string(' ', cli.blocksize - bufchars.Length));
                }
                Console.Write(" * ");

                foreach (byte b in buffer)
                {
                    Console.Write(b.ToString("X2"));
                }

                Console.WriteLine();


                offset += buffer.Length;
            }

            file.Close();
        }

        static void Main(string[] args)
        {
            Cli cli = new Cli(args);
            if (!cli.goodcli)
            {
                return;
            }
            foreach (string fn in cli.Arguments)
            {
                DumpFile(fn,cli);
            }
        }
    }
}
