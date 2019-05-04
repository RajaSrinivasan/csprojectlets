using System;
using numberslib;

namespace numtest
{
    class Program
    {
        static void Tests(int num)
        {
            Console.WriteLine($"No {num}-------------------------\nDigitsOf");
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
            bool perfect = UnaryTests.IsPerfect(num);
            Console.WriteLine($"IsPerfect {perfect}");
            bool harshad = UnaryTests.IsHarshad(num);
            Console.WriteLine($"IsHarshad {harshad}");
            bool happy = UnaryTests.IsHappy(num);
            Console.WriteLine($"IsHappy {happy}");
            bool kaprekar = UnaryTests.IsKaprekar(num);
            Console.WriteLine($"Iskaprekar {kaprekar}");
        }
        static void TwoaryTests(int num1, int num2)
        {
            Console.WriteLine($"Num1 {num1} Num2 {num2} -----------------");
            int gcd = TwoaryAlgorithms.Gcd(num1, num2);
            Console.WriteLine($"gcd {gcd}");
            bool mprime = TwoaryAlgorithms.MutualPrime(num1, num2);
            Console.WriteLine($"Mutually prime {mprime}");
        }

        static void SeriesTests(int num)
        {
            int[] fibs = Series.Fibonacci(num);
            Core.Show(fibs);
            GcdStruct[] gcds = Series.PairwiseGcd(fibs);
            Series.ShowGcdStructs(gcds);
        }

        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                int argno;
                argno = int.Parse(arg);
                Tests(argno);
                SeriesTests(argno);
            }
            if ((args.Length % 2) == 0)
            {
                for (int i=0; i<args.Length-1; i+=2)
                {
                    int arg1 = int.Parse(args[i]);
                    int arg2 = int.Parse(args[i+1]);
                    TwoaryTests(arg1, arg2);
                }
            }
        }
    }
}
