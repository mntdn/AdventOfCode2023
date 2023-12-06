using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2023
{
    internal class Day6 : AoCBase<long>
    {
        public Day6(string fileName) : base(fileName)
        {
        }

        public override long Part1()
        {
            long result = 0;
            List<List<long>> values = new List<List<long>>();
            foreach (var l in inputContent)
            {
                var du = l.Split(':')[1];
                string currentNumber = "";
                List<long> currentValues = new List<long>();
                for (int i = 0; i < du.Length; i++)
                {
                    if (du[i] != ' ')
                        currentNumber += du[i];
                    else if (currentNumber != "")
                    {
                        currentValues.Add(long.Parse(currentNumber));
                        currentNumber = "";
                    }
                }
                currentValues.Add(long.Parse(currentNumber));
                values.Add(currentValues);
            }

            List<long> nbVictorious = new List<long>();
            for (int i = 0; i < values[0].Count; i++)
            {
                long duration = values[0][i];
                long distance = values[1][i];
                long nbOk = 0;
                for(int j = 1; j < duration; j++)
                {
                    if(j*(duration - j) > distance)
                        nbOk++;
                }
                nbVictorious.Add(nbOk);
            }
            return nbVictorious.Aggregate((n1, n2) => n1 * n2);
        }

        public override long Part2()
        {
            long result = 0;
            List<List<long>> values = new List<List<long>>();
            foreach (var l in inputContent)
            {
                var du = l.Split(':')[1];
                string currentNumber = "";
                List<long> currentValues = new List<long>();
                for (int i = 0; i < du.Length; i++)
                {
                    if (du[i] != ' ')
                        currentNumber += du[i];
                }
                currentValues.Add(long.Parse(currentNumber));
                values.Add(currentValues);
            }

            long nbVictorious = 0;
            for (int i = 0; i < values[0].Count; i++)
            {
                long duration = values[0][i];
                long distance = values[1][i];
                for (int j = 1; j < duration; j++)
                {
                    if (j * (duration - j) > distance)
                        nbVictorious++;
                }
            }
            return nbVictorious;
        }
    }
}
