using System;
using System.Collections.Generic;

namespace Hackathon
{
    [Serializable]
    public class Term
    {
        private string m_value;

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        private List<TimeInVid> m_occurences;

        public List<TimeInVid> Occurences
        {
            get { return m_occurences; }
            private set { m_occurences = value; }
        }

        public int getNumOfOccurences()
        {
            if (Occurences == null)
                return 0;
            return Occurences.Capacity;
        }

        public Term(string term)
        {
            Value = term;
            m_occurences = new List<TimeInVid>();
        }

        public void AddOccurence(TimeSpan start, TimeSpan end)
        {
            TimeInVid timeOccured = new TimeInVid(start, end);
            Occurences.Add(timeOccured);
        }

    }
}