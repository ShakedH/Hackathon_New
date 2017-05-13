using Google.Cloud.Speech.V1;
using System;

namespace Hackathon
{
   public class APIGoogleClient
    {
        public String Convert(string FilePath)
        {
            string toReturn = "";
            var speech = SpeechClient.Create();
            var response = speech.Recognize(new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Flac,
                LanguageCode = "en",
            }, RecognitionAudio.FromFile(FilePath));
            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    toReturn += alternative.Transcript + " ";
                }
            }

            return toReturn;


        }

    }
}
