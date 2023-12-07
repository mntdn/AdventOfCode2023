using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2023
{
    internal class Day7 : AoCBase<long>
    {
        public Day7(string fileName) : base(fileName)
        {
        }

        public Dictionary<string, int> Cards = new Dictionary<string, int>()
        {
            {"A", 13 },
            {"K", 12},
            {"Q", 11},
            {"J", 10},
            {"T", 9},
            {"9", 8},
            {"8", 7},
            {"7", 6},
            {"6", 5},
            {"5", 4},
            {"4", 3},
            {"3", 2},
            {"2", 1}
        };

        public Dictionary<int, string> CardsRev = new Dictionary<int, string>()
        {
            {13 , "A" },
            {12, "K" },
            {11, "Q" },
            {10, "J" },
            {9, "T" },
            {8, "9" },
            {7, "8" },
            {6, "7" },
            {5, "6" },
            {4, "5" },
            {3, "4" },
            {2, "3" },
            {1, "2" }
        };

        public class GroupDefinition
        {
            public int Nb { get; set; }
            public int Val { get; set; }
            public GroupDefinition(int nb, int val)
            {
                Nb = nb;
                Val = val;
            }
        }

        public int GetHandValue(List<int> hand, string hString)
        {
            int result = 0;
            var l = hand.OrderBy(c => c).ToList();
            // <n, (nb, v)> : nb groups of n cards of value v
            Dictionary<int, GroupDefinition> cardGroups = new Dictionary<int, GroupDefinition>()
            {
                { 1, new GroupDefinition(0, 0) },
                { 2, new GroupDefinition(0, 0) },
                { 3, new GroupDefinition(0, 0) },
                { 4, new GroupDefinition(0, 0) },
                { 5, new GroupDefinition(0, 0) }
            };
            var test = l[0];
            int nbSimilar = 1;
            for (int i = 1; i < l.Count; i++)
            {
                if (l[i] == test)
                    nbSimilar++;
                else
                {
                    cardGroups[nbSimilar].Nb += 1;
                    cardGroups[nbSimilar].Val = test;
                    nbSimilar = 1;
                }
                test = l[i];
            }
            if (l[4] == l[3])
            {
                cardGroups[nbSimilar].Nb += 1;
                cardGroups[nbSimilar].Val = test;
            }
            else
            {
                cardGroups[1].Nb += 1;
                cardGroups[1].Val = l[4];
            }
            foreach (var g in cardGroups)
            {
                result += g.Value.Nb * (int)Math.Pow(10, g.Key-1);
            }
            Console.WriteLine($"{hString} : {result}  -> {string.Join(";", cardGroups.Select(c => $"{c.Key} = {c.Value.Nb}({c.Value.Val})"))}");
            return result;
        }

        public override long Part1()
        {
            long result = 0;

            Dictionary<string, int> handBids = new Dictionary<string, int>();
            Dictionary<string, (List<int>, int)> handValues = new Dictionary<string, (List<int>, int)>();
            foreach (var l in inputContent)
            {
                List<int> hand = l.Split(' ')[0].Select(h => Cards[h.ToString()]).ToList();
                int bid = int.Parse(l.Split(" ")[1]);
                handBids.Add(l.Split(' ')[0], bid);
                handValues.Add(l.Split(' ')[0], (hand, GetHandValue(hand, l.Split(' ')[0])));
            }
            Console.WriteLine("-------");
            int bidNumber = 1;
            foreach (var h in handBids.OrderBy(h => handValues[h.Key], 
                Comparer<(List<int>, int)>.Create((a,b) => { 
                    if(a.Item2 == b.Item2)
                    {
                        int final = 0;
                        for(int i = 0; i < 5; i++)
                        {
                            var comp = a.Item1[i] - b.Item1[i];
                            if (comp != 0)
                            {
                                final = comp;
                                break;
                            }
                        }
                        return final;
                    }
                    else
                        return a.Item2 - b.Item2;
                })))
            {
                string ok = string.Join("", h.Key.ToList().Select(hc => (int)Cards[hc.ToString()]).OrderBy(e => e).Select(hc => CardsRev[hc]).ToList());
                Console.WriteLine($"{h.Key}/{ok} : {h.Value} -- {result}");
                result += bidNumber * h.Value;
                bidNumber++;
            }
            return result;
        }
        public override long Part2()
        {
            throw new NotImplementedException();
        }
    }
}
