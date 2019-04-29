using System;
using numberslib;

namespace numtest
{
    class Program
    {
        static void Tests(int num)
        {
            Console.WriteLine($"No {num}\nDigitsOf");
            int[] t1 = Core.DigitsOf(num);
            Core.Show(t1);
            int val = Core.ValueDigitsOf(t1);
            Console.WriteLine($"Value is {val}");
            Console.WriteLine("DivisorsOf");
            int[] t2 = Core.DivisorsOf(num);
            Core.Show(t2);
            Console.WriteLine("PrimeFactorsOf");
            int[] t3 = Core.PrimeFactorsOf(num);
            Core.Show(t3);
            int prod = Core.ProductOf(t3);
            Console.WriteLine($"Product of all those {prod}");
            bool prime = UnaryTests.IsPrime(num);
            Console.WriteLine($"IsPrime {prime}");
        }
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                int argno;
                argno = int.Parse(arg);
                Tests(argno);
            }

        }
    }
}
