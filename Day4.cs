using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCode2023
{
    internal class Day4 : AoCBase<long>
    {
        public Day4(string fileName) : base(fileName)
        {
        }

        public override long Part1()
        {
            long result = 0;
            foreach (var l in inputContent)
            {
                var numbers = l.Split(':')[1];
                var winningNumbers = numbers.Split("|")[0].Split(' ').Where(n => !string.IsNullOrEmpty(n)).Select(n => int.Parse(n));
                var myNumbers = numbers.Split("|")[1].Split(' ').Where(n => !string.IsNullOrEmpty(n)).Select(n => int.Parse(n));
                result += (long)Math.Pow(2, myNumbers.Where(n => winningNumbers.Contains(n)).Count() - 1);
            }
            return result;
        }

        public void AddCardToStack(int card, Dictionary<int, int> cardStack)
        {
            if (cardStack.ContainsKey(card)) cardStack[card]++;
            else cardStack.Add(card, 1);
        }

        public override long Part2()
        {
            long result = 0;
            int cardNumber = 1;
            Dictionary<int, List<int>> cards = new Dictionary<int, List<int>>();
            foreach (var l in inputContent)
            {
                var numbers = l.Split(':')[1];
                var winningNumbers = numbers.Split("|")[0].Split(' ').Where(n => !string.IsNullOrEmpty(n)).Select(n => int.Parse(n));
                var myNumbers = numbers.Split("|")[1].Split(' ').Where(n => !string.IsNullOrEmpty(n)).Select(n => int.Parse(n));
                List<int> wonCards = new List<int>();
                for (int i = cardNumber + 1; i <= cardNumber + myNumbers.Where(n => winningNumbers.Contains(n)).Count(); i++)
                    wonCards.Add(i);

                cards.Add(cardNumber, wonCards);
                cardNumber++;
            }
            Dictionary <int, int> cardStack = new Dictionary<int, int>();
            int currentCardNo = 1;
            foreach (var card in cards)
            {
                card.Value.ForEach(c => AddCardToStack(c, cardStack));
                if (currentCardNo > 1 && cardStack.ContainsKey(currentCardNo))
                {
                    if(cards[currentCardNo].Count > 0)
                    {
                        for (int i = 0; i < cardStack[currentCardNo]; i++)
                        {
                            cards[currentCardNo].ForEach(c => AddCardToStack(c, cardStack));
                        }
                    }
                }
                AddCardToStack(currentCardNo, cardStack);
                currentCardNo++;
            }
            result = cardStack.Sum(c => c.Value);
            return result;
        }
    }
}
