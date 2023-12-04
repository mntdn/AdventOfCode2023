using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2023
{
    internal class Day3 : AoCBase<long>
    {
        public Day3(string fileName) : base(fileName)
        {
        }

        private long PartNumber(int lineNumber, int start, int end, long partNo)
        {
            start = start > 0 ? start - 1 : start;
            end = end < inputContent[lineNumber].Length - 1 ? end + 1 : end;
            int firstLine = lineNumber > 0 ? lineNumber - 1 : 0;
            int lastLine = lineNumber + 1 > inputContent.Count - 1 ? inputContent.Count - 1 : lineNumber + 1;
            bool isPartNumber = false;
            string show = System.Environment.NewLine;
            for (int i = firstLine; i <= lastLine; i++)
            {
                show += inputContent[i].Substring(start, end - start + 1) + System.Environment.NewLine;
                foreach (var c in inputContent[i].Substring(start, end - start + 1))
                {
                    if (!(c == '.' || (c >= 48 && c <= 57)))
                    {
                        isPartNumber = true;
                        break;
                    }
                }
                if (isPartNumber)
                    break;
            }
            show += partNo.ToString() + (isPartNumber ? " Y" : " N") + $" {lineNumber} {start} {end} -------------" + System.Environment.NewLine;
            //if(!isPartNumber)
                File.AppendAllText("c:\\temp\\r.txt", show);
            return isPartNumber ? partNo : 0;
        }

        public override long Part1()
        {
            long result = 0;
            int lineNumber = 0;
            File.WriteAllText("c:\\temp\\r.txt", "");
            foreach (var l in inputContent)
            {
                int start = -1;
                int end = -1;
                string currentNumber = "";
                for (var i = 0; i < l.Length; i++)
                {
                    var c = l[i];
                    if(c >= 48 && c <= 57)
                    {
                        if(start == -1) start = i;
                        currentNumber += c;
                        if(i == l.Length - 1) 
                        {
                            end = i - 1;
                            result += PartNumber(lineNumber, start, end, long.Parse(currentNumber));
                            start = -1; end = -1; currentNumber = "";
                        }
                    }
                    else
                    {
                        if (start >= 0 && end == -1)
                        {
                            end = i - 1;
                            result += PartNumber(lineNumber, start, end, long.Parse(currentNumber));
                            start = -1; end = -1; currentNumber = "";
                        }
                    }
                }
                lineNumber++;
            }
            return result;
        }

        private bool IsNumber(char c)
        {
            return c >= 48 && c <= 57;
        }

        // -1 start, 0 middle, 1 end
        private int DotPosition(string s)
        {
            if (s[0] == '.')
                return -1;
            else if (s[2] == '.')
                return 1;

            return 0;
        }

        private int GetNumber(int line, int col, bool? position)
        {
            int oldCol = col;
            int result = 0;
            if(position == null || position == false)
            {
                // possibly in the middle, go back to the start
                bool reachStart = false;
                while (IsNumber(inputContent[line][col]))
                {
                    if (col == 0)
                    {
                        reachStart = true;
                        break;
                    }
                    col--;
                }
                if (!reachStart) col++;
            }
            string finalNumber = "";
            while (IsNumber(inputContent[line][col]))
            {
                finalNumber += inputContent[line][col];
                if (col == inputContent[line].Length - 1)
                    break;
                col++;
            }
            result = int.Parse(finalNumber);
            return result;
        }

        public override long Part2()
        {
            long result = 0;
            int lineNumber = 0;
            File.WriteAllText("c:\\temp\\r.txt", "");
            foreach (var l in inputContent)
            {
                for (var i = 0; i < l.Length; i++)
                {
                    var c = l[i];
                    if (c == '*')
                    {
                        int start = i > 0 ? i - 1 : i;
                        int end = i < l.Length - 1 ? i + 1 : i;
                        int firstLine = lineNumber > 0 ? lineNumber - 1 : 0;
                        int lastLine = lineNumber + 1 > inputContent.Count - 1 ? inputContent.Count - 1 : lineNumber + 1;
                        int nbLines = lastLine - firstLine + 1;
                        // line, col, true = begin/false = end/null = none
                        List<(int, int, bool?)> partNos = new List<(int, int, bool?)>();
                        for (int j = firstLine; j <= lastLine; j++)
                        {
                            string curLine = inputContent[j].Substring(start, end - start + 1);
                            if((nbLines == 3 && (j == firstLine || j == lastLine)) || (nbLines == 2 && !curLine.Contains('*')))
                            {
                                int nbNumbers = curLine.Sum(c => IsNumber(c) ? 1 : 0);
                                if (nbNumbers > 0)
                                {
                                    if(nbNumbers == curLine.Length)
                                        partNos.Add((j, start, null));
                                    else if (curLine.Length == 2)
                                    {
                                        // only one number
                                        partNos.Add((j, IsNumber(curLine[0]) ? start : end, !IsNumber(curLine[0])));
                                    }
                                    else if(nbNumbers == 2)
                                    {
                                        // length == 3, search for . position
                                        if(DotPosition(curLine) == 0)
                                        {
                                            partNos.Add((j, start, false));
                                            partNos.Add((j, end, true));
                                        }
                                        else
                                        {
                                            partNos.Add((j, start + 1, DotPosition(curLine) > 0 ? false : true));
                                        }
                                    }
                                    else
                                    {
                                        // 1 number
                                        for(int k = 0; k < 3; k++)
                                        {
                                            if (IsNumber(curLine[k]))
                                            {
                                                partNos.Add((j, start + k, k == 1 ? null : (k == 0 ? false : true)));
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else if(curLine.Length == 3)
                            {
                                if (IsNumber(curLine[0]))
                                    partNos.Add((j, start, false));
                                if (IsNumber(curLine[2]))
                                    partNos.Add((j, end, true));
                            }
                            else if(curLine.Length == 2)
                            {
                                if (IsNumber(curLine[0]))
                                    partNos.Add((j, start, false));
                                if (IsNumber(curLine[1]))
                                    partNos.Add((j, end, true));
                            }
                        }
                        if (partNos.Count == 2)
                        {
                            int n1 = GetNumber(partNos[0].Item1, partNos[0].Item2, partNos[0].Item3);
                            int n2 = GetNumber(partNos[1].Item1, partNos[1].Item2, partNos[1].Item3);
                            result += n1 * n2;
                        }
                    }
                }
                lineNumber++;
            }
            return result;
        }
    }
}
