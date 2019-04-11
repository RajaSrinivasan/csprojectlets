using System;
namespace search
{
    public class Impl
    {
        Cli mycli;
        public Impl(Cli cli)
        {
            mycli = cli;
            if (mycli.verbose)
            {
                Console.WriteLine("Starting search");
            }
        }
    }
}
