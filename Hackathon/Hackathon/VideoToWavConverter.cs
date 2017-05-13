using System;
using System.Diagnostics;
using System.IO;

namespace Hackathon
{
    public class VideoToWavConverter
    {
        public string VideoFilePath { get; private set; }
        public string OutputAudioPath { get; set; }
        public string OutputFullPath { get; set; }
        public string CommandLine { get; private set; }

        public VideoToWavConverter(string inputVideoPath, string outputAudioPath)
        {
            if (!File.Exists(inputVideoPath))
                throw new ArgumentException("Input video file does not exists at: " + inputVideoPath);
            if (!Directory.Exists(outputAudioPath))
                throw new ArgumentException("Output path " + outputAudioPath + " does not exists");

            VideoFilePath = inputVideoPath;
            OutputAudioPath = outputAudioPath;
            if (!OutputAudioPath.EndsWith(@"\"))
                OutputAudioPath += @"\";
            string videoFileName = GetVideoFileName();
            OutputFullPath = OutputAudioPath + videoFileName + "_audio.wav";
            CommandLine = string.Format("C:\\ffmpeg\\bin\\ffmpeg -i \"{0}\" \"{1}\"", VideoFilePath, OutputFullPath);
        }

        private string GetVideoFileName()
        {
            int nameStartIndex = VideoFilePath.LastIndexOf(@"\") + 1;
            int nameLength = VideoFilePath.LastIndexOf(@".") - nameStartIndex;
            return VideoFilePath.Substring(nameStartIndex, nameLength);
        }

        public void Convert()
        {
            if (File.Exists(OutputFullPath))
                throw new Exception("Output audio file already exists at: " + OutputFullPath);

            string batchFileName = "converter.bat";
            using (StreamWriter sw = new StreamWriter(batchFileName))
            {
                sw.WriteLine(CommandLine);
            }
            Process p = new Process();
            p.StartInfo.FileName = batchFileName;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            File.Delete(batchFileName);
        }
    }
}