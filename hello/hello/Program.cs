using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace hello
//{
    class Program
    {
        static void PrintFile(String filename)
        {
            Console.WriteLine($"Printing file {filename}");
        }
        static void Main(string[] args)
        {
            foreach (String arg in args) {
                PrintFile(arg);
            }
        }
    }
//}
