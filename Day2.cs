using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCode2023
{
    internal class Day2 : AoCBase<long>
    {
        public Day2(string fileName) : base(fileName)
        {
        }

        public override long Part1()
        {
            long result = 0;
            var r = new Regex("Game ([0-9]+):(.*)");
            List<(int, Regex)> rColors = new List<(int, Regex)>(){
                (12, new Regex("([0-9]+) red")),
                (13, new Regex("([0-9]+) green")),
                (14, new Regex("([0-9]+) blue"))
            };
            foreach (var l in inputContent)
            {
                var m = r.Match(l);
                if (m.Success)
                {
                    int gameNumber = int.Parse(m.Groups[1].ToString());
                    string games = m.Groups[2].ToString();
                    bool isPossible = true;
                    foreach (var game in games.Split(';')) 
                    {
                        if (rColors.Any(reg =>
                        {
                            var m = reg.Item2.Match(game);
                            return m.Success && int.Parse(m.Groups[1].ToString()) > reg.Item1;
                        }))
                        {
                            isPossible = false;
                            break;
                        }
                    }
                    if(isPossible) { result += gameNumber; }
                }
            }
            return result;
        }

        public override long Part2()
        {
            long result = 0;
            var r = new Regex("Game ([0-9]+):(.*)");
            List<(string, Regex)> rColors = new List<(string, Regex)>(){
                ("r", new Regex("([0-9]+) red")),
                ("g", new Regex("([0-9]+) green")),
                ("b", new Regex("([0-9]+) blue"))
            };
            foreach (var l in inputContent)
            {
                var m = r.Match(l);
                if (m.Success)
                {
                    int gameNumber = int.Parse(m.Groups[1].ToString());
                    string games = m.Groups[2].ToString();
                    Dictionary<string, int> minNbs = new Dictionary<string, int>();
                    minNbs.Add("r", -1);
                    minNbs.Add("g", -1);
                    minNbs.Add("b", -1);
                    foreach (var game in games.Split(';'))
                    {
                        rColors.ForEach(reg =>
                        {
                            var m = reg.Item2.Match(game);
                            if (m.Success)
                            {
                                int nb = int.Parse(m.Groups[1].ToString());
                                minNbs[reg.Item1] = minNbs[reg.Item1] == -1 ? nb : (nb > minNbs[reg.Item1] ? nb : minNbs[reg.Item1]);
                            }
                        });
                    }
                    result += minNbs.Select(r => r.Value).Aggregate((a, b) => a * b);
                }
            }
            return result;
        }
    }
}
