using System;
using System.IO;

namespace Hackathon
{
    public class VideoToWavConverter
    {
        public string VideoFilePath { get; private set; }
        public string OutputFileName { get; set; }
        public string CommandLine { get; private set; }
        public string ffmpegPath
        {
            get { return @"C:\ffmpeg\bin\"; }
            private set { }
        }

        public VideoToWavConverter(string oVideoFilePath, string oOutoutFileName)
        {
            VideoFilePath = oVideoFilePath;
            OutputFileName = oOutoutFileName;
            CommandLine = "C:\\ffmpeg\\bin\\ffmpeg -i \" \"";
        }

        public void Convert()
        {
            throw new NotImplementedException();
            using (StreamWriter sw = new StreamWriter("executeConversion.bat"))
            {
            }
            //TEst
        }
    }
}