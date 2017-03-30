using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class Parser
    {
        private static char[] delimeters = new char[] { '.', ' ', ',', '-' };

        public static List<string> Parse(string text)
        {
            string[] stopWords = GetStopWords();
            string[] words = text.Split(delimeters);
            return words.Except(stopWords).ToList<string>();
        }

        private static string[] GetStopWords()
        {
            return File.ReadAllLines(@"Data\stop-word-list.txt");
        }
    }
}
