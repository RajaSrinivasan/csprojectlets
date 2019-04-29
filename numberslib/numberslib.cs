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
        static public int[] PrimeFactorsOf(int num)
        {
            List<int> factors = new List<int>();
            //int maxrange = (int)Math.Sqrt((double)num);
            int maxrange = num / 2;
            int currnum = num;
            factors.Add(1);
            for (int i=2; i <= maxrange; i++)
            {
                while ((currnum % i) == 0)
                {
                    factors.Add(i);
                    currnum = currnum / i;
                }
            }
            if (factors.Count == 1)
            {
                factors.Add(num);
            }
            return factors.ToArray();
        }

        static public int ProductOf(int[] factors)
        {
            int product=1;
            foreach (int i in factors)
            {
                product = product * i;
            }
            return product;
        }


        static public int Power(int num, int power)
        {
            switch (power)
            {
                case 1:
                    return num;
                case 2:
                    return num * num;
                case 3:
                    return num * Power(num, 2);
                case 4:
                    {
                        int sq = Power(num, 2);
                        return sq * sq;
                    }
                case 5:
                    {
                        int sq = Power(num, 2);
                        int cube = Power(num, 3);
                        return sq * cube;
                    }
                default:
                    {
                        int result = Power(num, 5);
                        for (int i=0; i<power-5; i++)
                        {
                            result = result * num;
                        }
                        return result;
                    }
            }
        }
        static public int[] Power(int[] nums, int power)
        {
            int[] powers = new int[nums.Length];
            for (int i=0; i<nums.Length; i++)
            {
                powers[i] = Power(nums[i], power);
            }
            return powers;
        }
    }

    public class UnaryTests
    {
        static public bool IsPrime(int num)
        {
            int[] factors = Core.PrimeFactorsOf(num);
            if (factors.Length == 2)
            {
                return true;
            }
            return false;
        }

    }
}
