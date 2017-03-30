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
        private static string[] stopWords;

        public static List<string> Parse(string text, string stopWordsPathDirectory)
        {
            string path = stopWordsPathDirectory + @"\stopWords.txt";
            if (stopWords == null)
                GetStopWords(path);
            List<string> words = text.Split(delimeters).ToList<string>();
            words = words.ConvertAll(d => d.ToLower());
            words = words.Except(stopWords).ToList<string>();
            words = words.Where(s => !string.IsNullOrEmpty(s)).ToList<string>();
            return words;
        }

        private static void GetStopWords(string stopWordsPath)
        {
            stopWords = File.ReadAllLines(stopWordsPath);
        }
    }
}
