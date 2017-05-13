using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    [Serializable]
    public class VideoDetails
    {
        public string Name
        {
            get;
            private set;
        }

        public List<string> Keywords
        {
            get;
            private set;
        }

        public VideoDetails(string name, List<string> keywords)
        {
            this.Name = name;
            this.Keywords = keywords;
        }
    }
}
