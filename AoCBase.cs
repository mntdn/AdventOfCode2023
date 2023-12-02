using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2023
{
    internal abstract class AoCBase<T>
    {
        public List<string> inputContent { get; set; }
        protected AoCBase(string fileName) 
        {
            inputContent = File.ReadAllLines(fileName, Encoding.UTF8).ToList();
        }
        abstract public T Part1();
        abstract public T Part2();
    }
}
