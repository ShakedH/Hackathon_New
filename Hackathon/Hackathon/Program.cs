using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class Program
    {
        private APIClient ApiClient;

        private VideoToWavConverter VidToSoundConverter;

        public Program(APIClient client, VideoToWavConverter converter)
        {
            this.ApiClient = client;
            this.VidToSoundConverter = converter;
        }

        public Dictionary<string, Term> ConvertVideo(string filePath)
        {
            // Convert video to audio:
            VidToSoundConverter.Convert();
            string audioFile = VidToSoundConverter.OutputFileName;
            List<string> fileNames;
            /*
                Split audioFile by 15 seconds. Save to fileNames
            */

            throw new NotImplementedException();
        }
    }
}
