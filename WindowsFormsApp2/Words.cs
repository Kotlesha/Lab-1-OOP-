using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WindowsFormsApp2
{
    class Words
    {
        private static Dictionary<string, decimal> words;

        public Words(string nameOfFile) => words = File.ReadAllLines(nameOfFile)
            .Select(line => line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(element => element[0], element => decimal.Parse(element[1].Replace('.', ',')));

        public static decimal GetValue(string description)
        {
            decimal totalValue = 0.0m, count = 0.0m;

            foreach (var word in words)
            {
                Regex pattern = new Regex(word.Key);
                MatchCollection countOfVowels = pattern.Matches(description);
                totalValue += words[word.Key] * countOfVowels.Count;
                count += countOfVowels.Count;
            }

            return count == decimal.Zero ? count : totalValue / count;
        }
    }
}
