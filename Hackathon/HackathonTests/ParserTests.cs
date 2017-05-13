using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.IO;

namespace HackathonTests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void Parse()
        {
            string text = "I like chocolate";
            char[] delimeters = new char[] { '.', ' ', ',', '-' };
            string[] stopWords = Hackathon.Properties.Resources.stop_words.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> words = text.Split(delimeters).ToList<string>();
            words = words.ConvertAll(d => d.ToLower());
            words = words.Except(stopWords).ToList<string>();
            words = words.Where(s => !string.IsNullOrEmpty(s)).ToList<string>();
            Assert.IsNotNull(stopWords);
        }

        IEnumerable<string> EnumerateLines(TextReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
