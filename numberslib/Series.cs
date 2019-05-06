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
    public struct NumPair
    {
        public int num1;
        public int num2;
    }

    public struct TaxicabNumber
    {
        public int Sum;
        public NumPair pair1;
        public NumPair pair2;
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

        static public Dictionary<Int64, List<NumPair>> GenerateTaxicabNumbers(int order, int limit)
        {
            int[] numbers = new int[limit];
            for (int num=0; num<limit; num++)
            {
                numbers[num] = num+1;
            }
            int[] numpowers = Core.Power(numbers, order);
            Dictionary<Int64, List<NumPair>> tcn = new Dictionary<Int64, List<NumPair>>();
            List<TaxicabNumber> taxicabNumbers = new List<TaxicabNumber>();

            NumPair pair;
            for (int row=0; row<numbers.Length; row++)
            {
                pair.num1 = numbers[row];
                for (int col=0; col < row; col++)
                {
                    pair.num2 = numbers[col];
                    Int64 sum = numpowers[row] + numpowers[col];
                    if (tcn.ContainsKey(sum))
                    {
                        List<NumPair> numpairlist;
                        tcn.TryGetValue(sum,out numpairlist);
                        numpairlist.Add(pair);
                    }
                    else
                    {
                        List<NumPair> numpairlist = new List<NumPair>();
                        numpairlist.Add(pair);
                        tcn.Add(sum, numpairlist);
                    }
                }
            }
           
            return tcn;
        }

        static public void Show(Dictionary<Int64,List<NumPair>> taxicabnumbers)
        {
            foreach (KeyValuePair<Int64, List<NumPair>> kvp in taxicabnumbers)
            {
                Int64 tcn = kvp.Key;
                if (kvp.Value.Count > 1)
                {
                    Console.WriteLine($"{tcn}");
                    foreach (NumPair numpair in kvp.Value)
                    {
                        Console.WriteLine($"\t{numpair.num1}\t {numpair.num2}");
                    }

                }
            }
        }
    }
}
