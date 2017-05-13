using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hackathon
{
    public class Converter
    {
        private APIGoogleClient ApiGoogleCLient;

        private VideoToWavConverter VidToSoundConverter;

        private const int TimeIntervals = 15;

        private const string OutputFilesFormat = "OutPut*.wav";

        private int m_FilesParsed;

        public int FilesParsed
        {
            get { return m_FilesParsed; }
            set { m_FilesParsed = value; }
        }

        public Converter(APIGoogleClient client)
        {
            this.ApiGoogleCLient = client;
        }


        /// <summary>
        /// Make Sure stopWordsFile is in the same place as file!
        /// </summary>
        public Dictionary<string, List<TimeInVid>> ConvertVideo(string filePath, int filesParsed = 0)
        {
            Dictionary<string, Dictionary<TimeSpan, string>> Sentences = new Dictionary<string, Dictionary<TimeSpan, string>>();
            Dictionary<string, List<TimeInVid>> Terms = new Dictionary<string, List<TimeInVid>>();

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
                    //string text = ApiClient.Convert(currentFile);
                    string text = ApiGoogleCLient.Convert(currentFile);
                    List<string> termsFound = Parser.Parse(text, directory.FullName);
                    foreach (string term in termsFound)
                    {
                        if (!Terms.ContainsKey(term))
                            Terms.Add(term, new List<TimeInVid>());

                        TimeInVid time = new TimeInVid(start, end);
                        Terms[term].Add(time);

                        MatchCollection matches = Regex.Matches(text, @"(?:\S+\s)?\S*(?:\S+\s)?\S*" + term + @"\S*(?:\s\S+)?\S*(?:\s\S+)?", RegexOptions.IgnoreCase);
                        string sentence = matches[0].Value;
                        if (!Sentences.ContainsKey(term))
                            Sentences.Add(term, new Dictionary<TimeSpan, string>());
                        Sentences[term].Add(time.Start, sentence);
                    }
                }
                catch (Exception)
                {
                    //Console.WriteLine(e.Message);
                }
                finally
                {
                    FilesParsed++;
                    start = end;
                    end = start.Add(toAddSpan);
                }
            }

        }

    }
}
