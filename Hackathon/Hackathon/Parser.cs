using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class Parser
    {
        private static char[] delimeters = new char[] { '.', ' ', ',', '-' };

        public static List<string> Parse(string text, string stopWordsPathDirectory)
        {
            string path = stopWordsPathDirectory + @"\stopWords.txt";
            string[] stopWords = GetStopWords(path);
            string[] words = text.Split(delimeters);
            List<string> wordsList = words.Except(stopWords).ToList<string>();
            wordsList = wordsList.ConvertAll(d => d.ToLower());
            wordsList = wordsList.Where(s => !string.IsNullOrEmpty(s)).ToList<string>();
            return wordsList;
        }

        private static string[] GetStopWords(string stopWordsPath)
        {
            return File.ReadAllLines(stopWordsPath);
        }
    }
}
