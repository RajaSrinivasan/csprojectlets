using System;

namespace search
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Cli cli = new Cli(args);
            Impl impl = new Impl(cli);
        }
    }
}
