using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2023
{
    internal class Day1: AoCBase<long>
    {
        public Day1(string fileName) : base(fileName)
        {
        }

        public override long Part1()
        {
            long result = 0;
            foreach (var l in inputContent)
            {
                int start = 0; int end = 0;
                for (int i = 0; i < l.Length; i++)
                {
                    if (l[i] >= 48 && l[i] <= 57)
                    {
                        start = l[i] - 48;
                        break;
                    }
                }
                for (int j = l.Length - 1; j >= 0; j--)
                {
                    if (l[j] >= 48 && l[j] <= 57)
                    {
                        end = l[j] - 48;
                        break;
                    }
                }
                result += (start * 10) + end;
                Console.WriteLine($"{l} -> {(start * 10) + end} = {result}");
            }
            return result;
        }

        public override long Part2()
        {
            List<string> numbers = new List<string>() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            long result = 0;
            foreach (var l in inputContent)
            {
                string localL = l;
                List<(int, int)> toInsert = new List<(int, int)>();
                for (int k = 0; k < numbers.Count; k++)
                {
                    bool found = true;
                    int index = 0;
                    while (found)
                    {
                        int pos = localL.IndexOf(numbers[k], index);
                        if (pos >= 0)
                        {
                            index = pos + 1;
                            toInsert.Add((pos, k + 1));
                            if (index == localL.Length - 1)
                                found = false;
                        }
                        else found = false;
                    }
                }
                int g = 0;
                foreach (var item in toInsert.OrderBy(a => a.Item1))
                {
                    localL = localL.Substring(0, item.Item1 + g) + item.Item2 + localL.Substring(item.Item1 + g);
                    g++;
                }
                int start = 0; int end = 0;
                int i = 0;
                for (; i < localL.Length; i++)
                {
                    if (localL[i] >= 48 && localL[i] <= 57)
                    {
                        start = localL[i] - 48;
                        break;
                    }
                }
                int j = localL.Length - 1;
                for (; j >= 0; j--)
                {
                    if (localL[j] >= 48 && localL[j] <= 57)
                    {
                        end = localL[j] - 48;
                        break;
                    }
                }
                result += (start * 10) + end;
                Console.WriteLine($"{l} = {localL} -> {(start * 10) + end} = {result}");
            }
            return result;
        }
    }
}
