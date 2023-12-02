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
            List<Regex> rColors = new List<Regex>(){
                new Regex("([0-9]+) red"),
                new Regex("([0-9]+) green"),
                new Regex("([0-9]+) blue")
            };
            foreach (var l in inputContent)
            {
                var m = r.Match(l);
                if (m.Success)
                {
                    int gameNumber = int.Parse(m.Groups[1].ToString());
                    string games = m.Groups[2].ToString();
                    foreach (var game in games.Split(';')) 
                    {
                        var vals = rColors.Select(reg => reg.Match(game))
                    }
                }
            }
            return result;
        }

        public override long Part2()
        {
            throw new NotImplementedException();
        }
    }
}
