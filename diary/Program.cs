using System;

namespace diary
{
    class Program
    {
        static void Main(string[] args)
        {
            Cli cli = new Cli(args);
            Impl impl = new Impl(cli);
            impl.Execute();
        }
    }
}
