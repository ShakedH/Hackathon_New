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
        private static string[] stopWords = Properties.Resources.stop_words.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        public static List<string> Parse(string text)
        {
            List<string> words = text.Split(delimeters).ToList();
            words = words.ConvertAll(d => d.ToLower());
            words = words.Except(stopWords).ToList();
            words = words.Where(s => !string.IsNullOrEmpty(s)).ToList();
            return words;
        }
    }
}
