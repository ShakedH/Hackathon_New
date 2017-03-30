﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
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

        private Dictionary<TermTimeKey, string> m_Sentences = new Dictionary<TermTimeKey, string>();

        public Dictionary<TermTimeKey, string> Sentences
        {
            get { return m_Sentences; }
            private set { m_Sentences = value; }
        }




        public Program(APIClient client)
        {
            this.ApiClient = client;
        }

        public Dictionary<string, List<TimeInVid>> GetMostFrequentStrings(int numOfResults)
        {
            return (from entry in Terms orderby entry.Value.Capacity descending select entry)
                   .ToDictionary(pair => pair.Key, pair => pair.Value).Take(numOfResults).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public List<TimeInVid> SearchWord(string term)
        {
            term = term.ToLower();
            if (Terms.ContainsKey(term))
                return Terms[term];
            else
                return null;
        }

        /// <summary>
        /// Make Sure stopWordsFile is in the same place as file!
        /// </summary>
        public Dictionary<string, List<TimeInVid>> ConvertVideo(string filePath, int filesParsed = 0)
        {
            this.FilesParsed = filesParsed;
            if (FilesParsed > 0)
                LoadFromFile(filePath.Replace(".mp4", ".bin"));

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
                    string text = ApiClient.Convert(currentFile);
                    List<string> termsFound = Parser.Parse(text, directory.FullName);
                    foreach (string term in termsFound)
                    {
                        if (!Terms.ContainsKey(term))
                            Terms.Add(term, new List<TimeInVid>());

                        TimeInVid time = new TimeInVid(start, end);
                        Terms[term].Add(time);

                        MatchCollection matches = Regex.Matches(text, @"(?:\S+\s)?\S*(?:\S+\s)?\S*" + term + @"\S*(?:\s\S+)?\S*(?:\s\S+)?", RegexOptions.IgnoreCase);
                        string sentence = matches[0].Value;
                        Sentences.Add(new TermTimeKey(term, time), sentence);
                    }
                }
                catch (Exception e)
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

            return Terms;
        }

        public void SaveToFile(string Directory)
        {
            string TermsFilePath = Directory + @"\Terms.bin";
            using (FileStream stream = File.Open(TermsFilePath, FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, Terms);
            }

            string SentencesFilePath = Directory + @"\Sentences.bin";
            using (FileStream stream = File.Open(SentencesFilePath, FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, Sentences);
            }
        }

        public void LoadFromFile(string Directory)
        {
            string TermsFilePath = Directory + @"\Terms.bin";
            using (FileStream stream = File.Open(TermsFilePath, FileMode.Open))
            {
                BinaryFormatter reader = new BinaryFormatter();
                Terms = (Dictionary<string, List<TimeInVid>>)reader.Deserialize(stream);
            }

            string SentencesFilePath = Directory + @"\Sentences.bin";
            using (FileStream stream = File.Open(SentencesFilePath, FileMode.Open))
            {
                BinaryFormatter reader = new BinaryFormatter();
                Sentences = (Dictionary<TermTimeKey, string>)reader.Deserialize(stream);
            }
        }
    }

    [Serializable]
    public class TermTimeKey
    {
        private string m_term;

        public string Term
        {
            get { return m_term; }
            set { m_term = value; }
        }

        private TimeInVid m_time;

        public TimeInVid Time
        {
            get { return m_time; }
            set { m_time = value; }
        }

        public TermTimeKey(string term, TimeInVid time)
        {
            Term = term;
            Time = time;
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            TermTimeKey other = (TermTimeKey)obj;
            return Term.Equals(other.Term) && Time.Equals(other.Time);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }
    }

}
