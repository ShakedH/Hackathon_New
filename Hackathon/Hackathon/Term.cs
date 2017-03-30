using System;
using System.Collections.Generic;

namespace Hackathon
{
    public class Term
    {
        private string m_value;

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        private Dictionary<Lecture, List<TimeInVid>> m_occurences;

        public Dictionary<Lecture, List<TimeInVid>> Occurences
        {
            get { return m_occurences; }
            private set { m_occurences = value; }
        }

        public Term(string term)
        {
            m_occurences = new Dictionary<Lecture, List<TimeInVid>>();
        }

        public void AddOccurence(Lecture lecture, TimeSpan start, TimeSpan end)
        {
            TimeInVid timeOccured = new TimeInVid(start, end);

            if (!Occurences.ContainsKey(lecture))
                Occurences.Add(lecture, new List<TimeInVid>());

            if (!Occurences[lecture].Contains(timeOccured))
                Occurences[lecture].Add(timeOccured);

        }
    }
}