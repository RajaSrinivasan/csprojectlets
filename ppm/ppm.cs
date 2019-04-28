using System;
using System.Security.Cryptography;
using System.Text;

namespace ppm
{
    class Program
    {
        static void Main(string[] args)
        {

            Cli cli = new Cli(args);

            Passbase passbase = new Passbase(cli);
            string cmd = cli.Command("create,add,update,show,list,test");
            if (cmd.Length > 1)
            {
                string dbpassword = cli.GetPassword("Password:");
                passbase.SetPassword(dbpassword);
            }
            else
            {
                return;
            }
            switch (cmd)
            {
                case "create":
                    passbase.Create();
                    break;
                case "list":
                    passbase.List();
                    break;
                case "add":
                    passbase.Add();
                    if (cli.verbose)
                    {
                        passbase.List();
                    }
                    break;
                case "update":
                    passbase.Update();
                    break;
                case "show":
                    passbase.Show();
                    break;
                case "test":
                    passbase.Test();
                    break;
                default:
                    Console.WriteLine($"{cmd} is not implemented");
                    break;
            }
        }
    }
}
