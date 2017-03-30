using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class Program
    {
        private APIClient ApiClient;

        private VideoToWavConverter VidToSoundConverter;

        private const int TimeIntervals = 15;

        private const string OutputFilesFormat = "output*.txt";

        private Dictionary<Term, List<TimeInVid>> m_terms;

        public Dictionary<Term, List<TimeInVid>> Terms
        {
            get { return m_terms; }
            set { m_terms = value; }
        }

        public Program(APIClient client, VideoToWavConverter converter)
        {
            this.ApiClient = client;
            this.VidToSoundConverter = converter;
        }

        public Dictionary<Term, List<TimeInVid>> ConvertVideo(string filePath)
        {
            Dictionary<Term, List<TimeInVid>> termsToReturn = new Dictionary<Term, List<TimeInVid>>();
            if (!File.Exists(filePath))
                throw new ArgumentException(string.Format("File {0} not found!", filePath));


            // Convert video to audio:
            VidToSoundConverter.Convert();
            string fullAudioPath = VidToSoundConverter.OutputFullPath;
            // Split audio:
            WavSplitter.Split(TimeIntervals, fullAudioPath);
            DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(filePath));
            // Find all split files:
            FileInfo[] taskFiles = directory.GetFiles(OutputFilesFormat);

            TimeSpan toAddSpan = new TimeSpan(0, 0, TimeIntervals);
            TimeSpan start = new TimeSpan(0, 0, 0);
            TimeSpan end = start.Add(toAddSpan);

            for (int i = 0; i < taskFiles.Length; i++)
            {
                // find file #(i+1):
                string currentFile = directory.FullName + "\\" + OutputFilesFormat.Replace("*", (i + 1).ToString());
                string text = ApiClient.Convert(currentFile);
                // Process text to termsFound
                List<string> termsFound = Parser.Parse(text);
                foreach (string termVal in termsFound)
                {
                    Term term = new Term(termVal);
                    if (!termsToReturn.ContainsKey(term))
                        termsToReturn.Add(term, new List<TimeInVid>());
                    termsToReturn[term].Add(new TimeInVid(start, end));
                }
                start = end;
                end = start.Add(toAddSpan);
            }

            Terms = termsToReturn;
            return termsToReturn;
        }

        public void SaveToFile(string filePath)
        {
            using (FileStream stream = File.Open(filePath, FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, Terms);
            }
        }

        public void LoadFromFile(string filePath)
        {
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                BinaryFormatter reader = new BinaryFormatter();
                Terms = (Dictionary<Term, List<TimeInVid>>)reader.Deserialize(stream);
            }
        }
    }
}
