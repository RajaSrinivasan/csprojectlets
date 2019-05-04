using System;
using System.Collections.Generic;

namespace numberslib
{
    public struct GcdStruct
    {
        public int num1;
        public int num2;
        public int gcd;
    }
    public class Series
    {
        public Series()
        {
        }
        static public int[] Fibonacci(int count)
        {
            int[] series = new int[count];
            series[0] = 0;
            series[1] = 1;
            for (int i=2; i < count; i++)
            {
                series[i] = series[i - 1] + series[i - 2];
            }
            return series;
        }
        static public void ShowGcdStructs(GcdStruct[] gcds)
        {
            foreach (GcdStruct gcd in gcds)
            {
                Console.WriteLine($"{gcd.num1} \t{gcd.num2} \t: {gcd.gcd}");
            }
        }

        static public GcdStruct[] PairwiseGcd(int []numbers)
        {
            List<GcdStruct> resultlist = new List<GcdStruct>();
            GcdStruct gcdforpair;
            for (int row=0; row<numbers.Length; row++)
            {
                if (numbers[row] < 1) continue;
                gcdforpair.num1 = numbers[row];
                for (int col=0; col < row ; col++)
                {
                    if (numbers[col] < 1) continue;
                    gcdforpair.num2 = numbers[col];
                    gcdforpair.gcd = TwoaryAlgorithms.Gcd(numbers[row], numbers[col]);
                    if (gcdforpair.gcd != 1) resultlist.Add(gcdforpair);
                }
            }
            return resultlist.ToArray();
        }
    }
}
