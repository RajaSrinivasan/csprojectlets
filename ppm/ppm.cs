using System;
using System.Security.Cryptography;
using System.Text;

namespace ppm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Cli cli = new Cli(args);
            string dbpassword = cli.GetPassword("Password:");
            Console.WriteLine($"Password was {dbpassword}");
            Passbase passbase = new Passbase(cli);
            passbase.SetPassword(dbpassword);
            Console.WriteLine(passbase.Password);

            string cmd = cli.Command("create,add,update,list");
            Console.WriteLine($"Command {cmd}");
        }
    }
}
