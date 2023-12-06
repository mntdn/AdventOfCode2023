using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCode2023
{
    internal class Day5 : AoCBase<long>
    {
        public Day5(string fileName) : base(fileName)
        {
        }

        public long ProcessMapping(long param, long dest, long source, long nb)
        {
            long result = 0;
            if(param >= source && param < source + nb)
                result = dest + param - source;
            return result;
        }

        public override long Part1()
        {
            List<long> seeds = new List<long>();
            var rSeed = new Regex(@"seeds: (.*)");
            var rMap = new Regex(@"(\w+-to-\w+) map:");
            var rNumbers = new Regex(@"(\d+) (\d+) (\d+)");
            Dictionary<string, List<(long, long, long)>> mappings = new Dictionary<string, List<(long, long, long)>>();
            string curMapName = "";
            foreach (var l in inputContent)
            {
                var mSeed = rSeed.Match(l);
                var mMap = rMap.Match(l);
                var mNumbers = rNumbers.Match(l);
                if (mSeed.Success)
                {
                    seeds = mSeed.Groups[1].ToString().Split(" ").Where(s => !string.IsNullOrEmpty(s)).Select(s => long.Parse(s)).ToList();
                }
                else if(mMap.Success)
                {
                    curMapName = mMap.Groups[1].ToString();
                }
                else if (mNumbers.Success)
                {
                    if (!mappings.ContainsKey(curMapName))
                        mappings.Add(curMapName, new List<(long, long, long)>());
                    mappings[curMapName].Add((long.Parse(mNumbers.Groups[1].ToString()), long.Parse(mNumbers.Groups[2].ToString()), long.Parse(mNumbers.Groups[3].ToString())));
                }
            }
            List<long> locations = new List<long>();
            foreach (var s in seeds)
            {
                long finalLocation = s;
                foreach (var m in mappings) 
                {
                    foreach (var c in m.Value)
                    {
                        var r = ProcessMapping(finalLocation, c.Item1, c.Item2, c.Item3);
                        if (r > 0)
                        {
                            finalLocation = r;
                            break;
                        }
                    }
                }
                locations.Add(finalLocation);
            }
            return locations.Min();
        }

        public override long Part2()
        {
            List<long> seeds = new List<long>();
            var rSeed = new Regex(@"seeds: (.*)");
            var rMap = new Regex(@"(\w+-to-\w+) map:");
            var rNumbers = new Regex(@"(\d+) (\d+) (\d+)");
            Dictionary<string, List<(long, long, long)>> mappings = new Dictionary<string, List<(long, long, long)>>();
            string curMapName = "";
            foreach (var l in inputContent)
            {
                var mSeed = rSeed.Match(l);
                var mMap = rMap.Match(l);
                var mNumbers = rNumbers.Match(l);
                if (mSeed.Success)
                {
                    seeds = mSeed.Groups[1].ToString().Split(" ").Where(s => !string.IsNullOrEmpty(s)).Select(s => long.Parse(s)).ToList();
                }
                else if (mMap.Success)
                {
                    curMapName = mMap.Groups[1].ToString();
                }
                else if (mNumbers.Success)
                {
                    if (!mappings.ContainsKey(curMapName))
                        mappings.Add(curMapName, new List<(long, long, long)>());
                    mappings[curMapName].Add((long.Parse(mNumbers.Groups[1].ToString()), long.Parse(mNumbers.Groups[2].ToString()), long.Parse(mNumbers.Groups[3].ToString())));
                }
            }

            List<(long, long)> ranges = new List<(long, long)>();
            long nbToTry = 0;
            for (int i = 0; i < seeds.Count(); i += 2)
            {
                long start = seeds[i];
                long end = seeds[i] + seeds[i + 1];
                ranges.Add((start, end));

                nbToTry += seeds[i + 1];
            }

            bool found = false;
            List<List<(long, long, long)>> reverseMappings = mappings.Select(m => m.Value).Reverse().ToList();
            long location = 0;
            while(!found)
            {
                long finalLocation = location;
                foreach (var m in reverseMappings)
                {
                    foreach (var c in m)
                    {
                        var r = ProcessMapping(finalLocation, c.Item2, c.Item1, c.Item3);
                        if (r > 0)
                        {
                            finalLocation = r;
                            break;
                        }
                    }
                }
                if (ranges.Exists(r => finalLocation >= r.Item1 && finalLocation < r.Item2))
                    found = true;
                else 
                    location++;
            }
            return location;
        }
    }
}
