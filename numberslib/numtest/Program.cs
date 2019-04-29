using System;
using numberslib;

namespace numtest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("No 1056\nDigitsOf");
            int[] t1 = Core.DigitsOf(1056);
            Core.Show(t1);
            int val = Core.ValueDigitsOf(t1);
            Console.WriteLine($"Value is {val}");
            Console.WriteLine("DivisorsOf");
            int[] t2 = Core.DivisorsOf(1056);
            Core.Show(t2);

        }
    }
}
