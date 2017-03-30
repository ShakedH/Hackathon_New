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

        private const string OutputFilesFormat = "OutPut*.wav";

        private Dictionary<string, List<TimeInVid>> m_terms = new Dictionary<string, List<TimeInVid>>();

        public Dictionary<string, List<TimeInVid>> Terms
        {
            get { return m_terms; }
            set { m_terms = value; }
        }

        private int m_FilesParsed;

        public int FilesParsed
        {
            get { return m_FilesParsed; }
            set { m_FilesParsed = value; }
        }

        public Program(APIClient client)
        {
            this.ApiClient = client;
        }

        public Dictionary<string, List<TimeInVid>> ConvertVideo(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException(string.Format("File {0} not found!", filePath));
            VidToSoundConverter = new VideoToWavConverter(filePath, Path.GetDirectoryName(filePath));

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
            FilesParsed = 0;
            for (int i = 0; i < FilesParsed; i++)
            {
                start = end;
                end = start.Add(toAddSpan);
            }
            for (int i = FilesParsed; i < taskFiles.Length; i++)
            {
                // find file #(i+1):
                string currentFile = directory.FullName + "\\" + OutputFilesFormat.Replace("*", (i + 1).ToString());
                try
                {
                    string text = ApiClient.Convert(currentFile);
                    List<string> termsFound = Parser.Parse(text, directory.FullName);
                    foreach (string term in termsFound)
                    {

                        if (!Terms.ContainsKey(term))
                            Terms.Add(term, new List<TimeInVid>());
                        Terms[term].Add(new TimeInVid(start, end));
                    }
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
                finally
                {
                    FilesParsed++;
                    start = end;
                    end = start.Add(toAddSpan);
                }
            }

            return Terms;
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
                Terms = (Dictionary<string, List<TimeInVid>>)reader.Deserialize(stream);
            }
        }
    }
}
