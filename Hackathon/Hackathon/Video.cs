using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    [Serializable]
    public class Video
    {
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        private Dictionary<string, List<TimeInVid>> m_Terms = new Dictionary<string, List<TimeInVid>>();
        public Dictionary<string, List<TimeInVid>> Terms
        {
            get { return m_Terms; }
            set { m_Terms = value; }
        }
        private Dictionary<string, Dictionary<TimeSpan, string>> m_Sentences = new Dictionary<string, Dictionary<TimeSpan, string>>();
        public Dictionary<string, Dictionary<TimeSpan, string>> Sentences
        {
            get { return m_Sentences; }
            private set { m_Sentences = value; }
        }
        private VideoMetadata m_Metadata = new VideoMetadata("Engineering", "ISSE", "Communication", "Dr. Kobi Gal");
        public VideoMetadata Metadata
        {
            get { return m_Metadata; }
            set { m_Metadata = value; }
        }

        public Video(string name)
        {
            this.Name = name;
        }

        public string GetSentence(string term, TimeInVid time)
        {
            if (Sentences.ContainsKey(term) && Sentences[term].ContainsKey(time.Start))
                return Sentences[term][time.Start];
            return "";
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
            return new List<TimeInVid>();
        }

        public void SaveToFile(string filePath)
        {
            string saveFilePath = filePath + @"\" + this.Name + ".bin";
            using (FileStream stream = File.Open(saveFilePath, FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, this);
            }
        }

        public static Video LoadVideoFromFile(string fullFilePath)
        {
            Video video = null;
            using (FileStream stream = File.Open(fullFilePath, FileMode.Open))
            {
                BinaryFormatter reader = new BinaryFormatter();
                video = (Video)reader.Deserialize(stream);
            }
            return video;
        }

        public static Video LoadVideoFromResource(string resourceName)
        {
            byte[] data = (byte[])Properties.Resources.ResourceManager.GetObject(resourceName, Properties.Resources.Culture);
            Video video = null;
            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter reader = new BinaryFormatter();
                video = (Video)reader.Deserialize(stream);
            }
            return video;
        }
    }
}
