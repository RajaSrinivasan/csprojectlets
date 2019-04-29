using System;
using System.Collections.Generic;

namespace numberslib
{
    public class Core
    {
        static public void Show(int[] seq)
        {
            Console.Write("[");
            foreach (int num in seq)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine("]");
        }
        static public int[] DigitsOf(int num)
        {
            List<int> digs = new List<int>();
            int orig = num;
            while (orig > 0)
            {
                int dig = orig % 10;
                digs.Add(dig);
                orig = orig / 10;
            }
            digs.Reverse();
            return digs.ToArray();
                
        }
        static public int ValueDigitsOf(int[] digits)
        {
            int value = 0;
            foreach (int d in digits)
            {
                value = value * 10 + d;
            }
            return value;
        }

        static public int[] DivisorsOf(int num)
        {
            List<int> divisors = new List<int>() ;
            int maxrange = (int)(Math.Sqrt((double)num));
            for (int i =1; i<= maxrange; i++)
            {
                if ((num % i) == 0)
                {
                    int quotient = num / i;
                    divisors.Add(i);
                    divisors.Add(quotient);
                }
            }
            divisors.Sort();
            return divisors.ToArray();
        }
    }
}
