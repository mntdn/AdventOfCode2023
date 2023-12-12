using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCode2023
{
    internal class Day8 : AoCBase<long>
    {
        public Day8(string fileName) : base(fileName)
        {
        }

        public override long Part1()
        {
            long result = 0;
            string format = "";
            var r = new Regex(@"(\w+) = \((\w+), (\w+)\)");
            Dictionary<string, (string, string)> nodes = new Dictionary<string, (string, string)>();
            string firstNode = "";
            string lastNode = "";
            string curNode = "";
            foreach (var l in inputContent)
            {
                if(string.IsNullOrEmpty(format))
                {
                    format = l;
                }
                else
                {
                    var m = r.Match(l);
                    if (m.Success)
                    {
                        curNode = m.Groups[1].ToString();
                        nodes.Add(curNode, (m.Groups[2].ToString(), m.Groups[3].ToString()));
                        if(string.IsNullOrEmpty(firstNode) ) { firstNode = curNode; }
                    }
                }
            }
            lastNode = "ZZZ";
            bool isDone = false;
            var instructions = format.Select(f => f.ToString()).ToList();
            long nbSteps = 0;
            // key, (arrival after sequence, true if contains endNode)
            Dictionary<string, (string, bool)> optimisedNodes = new Dictionary<string, (string, bool)>();
            foreach(var n in nodes)
            {
                string currentNode = n.Key;
                bool containsEndNode = false;
                foreach (var i in instructions) 
                {
                    currentNode = i == "L" ? nodes[currentNode].Item1 : nodes[currentNode].Item2;
                    if(currentNode == lastNode)
                        containsEndNode = true;
                }
                optimisedNodes.Add(n.Key, (currentNode, containsEndNode));
            }

            string currentFinalNode = "AAA";
            while (!isDone)
            {
                if (optimisedNodes[currentFinalNode].Item2)
                {
                    foreach (var i in instructions)
                    {
                        currentFinalNode = i == "L" ? nodes[currentFinalNode].Item1 : nodes[currentFinalNode].Item2;
                        nbSteps++;
                        if (currentFinalNode == lastNode) 
                        {

                            isDone = true;
                            break;
                        }
                    }
                }
                else
                {
                    currentFinalNode = optimisedNodes[currentFinalNode].Item1;
                    nbSteps += instructions.Count();
                }
            }
            return nbSteps;
        }

        public int LeastCommon(List<List<int>> values)
        {
            Dictionary<int, int> nbZ = new Dictionary<int, int>();
            foreach (var v in values)
            {
                foreach (var i in v)
                {
                    if(!nbZ.ContainsKey(i))
                        nbZ.Add(i, 0);
                    nbZ[i]++;
                }
            }
            var filterDic = nbZ.Where(n => n.Value == values.Count);
            if (filterDic.Count() == 0)
                return -1;
            else 
                return filterDic.Select(n => n.Key).OrderBy(n => n).First();
        }

        public override long Part2()
        {
            long result = 0;
            string format = "";
            var r = new Regex(@"(\w+) = \((\w+), (\w+)\)");
            Dictionary<string, (string, string)> nodes = new Dictionary<string, (string, string)>();
            string curNode = "";
            foreach (var l in inputContent)
            {
                if (string.IsNullOrEmpty(format))
                {
                    format = l;
                }
                else
                {
                    var m = r.Match(l);
                    if (m.Success)
                    {
                        curNode = m.Groups[1].ToString();
                        nodes.Add(curNode, (m.Groups[2].ToString(), m.Groups[3].ToString()));
                    }
                }
            }
            bool isDone = false;
            var instructions = format.Select(f => f.ToString()).ToList();
            List<string> startNodes = nodes.Select(n => n.Key).Where(n => n[2] == 'A').ToList();

            long nbSteps = 0;
            // key, (arrival after sequence, List of positions with end node)
            Dictionary<string, (string, List<int>)> optimisedNodes = new Dictionary<string, (string, List<int>)>();
            foreach (var n in nodes)
            {
                string currentNode = n.Key;
                List<int> okPos = new List<int>();
                int curPos = 0;
                foreach (var i in instructions)
                {
                    currentNode = i == "L" ? nodes[currentNode].Item1 : nodes[currentNode].Item2;
                    if (currentNode[2] == 'Z')
                        okPos.Add(curPos);
                    curPos++;
                }
                optimisedNodes.Add(n.Key, (currentNode, okPos));
            }

            while (!isDone)
            {
                List<List<int>> zPos = new List<List<int>>();
                for(int j = 0; j < startNodes.Count; j++)
                {
                    startNodes[j] = optimisedNodes[startNodes[j]].Item1;
                    zPos.Add(optimisedNodes[startNodes[j]].Item2);
                }
                nbSteps += instructions.Count();
                if (LeastCommon(zPos) >= 0)
                {
                    isDone = true;
                    nbSteps += LeastCommon(zPos) + 1;
                    break;
                }
            }
            return nbSteps;
        }
    }
}
