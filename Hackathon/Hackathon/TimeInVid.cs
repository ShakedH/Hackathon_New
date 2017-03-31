using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    [Serializable]
    public class TimeInVid
    {
        private TimeSpan m_start;

        public TimeSpan Start
        {
            get { return m_start; }
            set { m_start = value; }
        }

        private TimeSpan m_end;

        public TimeSpan End
        {
            get { return m_end; }
            set { m_end = value; }
        }

        public TimeInVid(TimeSpan start, TimeSpan end)
        {
            this.Start = start;
            this.End = end;
        }

        public override string ToString()
        {
            return Start.ToString();
        }
    }
}
