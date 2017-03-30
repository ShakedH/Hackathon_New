using System.IO;
using System.Diagnostics;
using System;

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
                throw new ArgumentException("Input video file path is not valid");
            VideoFilePath = inputVideoPath;
            OutputAudioPath = outputAudioPath;
            if (!OutputAudioPath.EndsWith(@"\"))
                OutputAudioPath += @"\";
            OutputFullPath = OutputAudioPath + "output_audio.wav";
            CommandLine = string.Format("C:\\ffmpeg\\bin\\ffmpeg -i \"{0}\" \"{1}\"", VideoFilePath, OutputFullPath);
        }

        public void Convert()
        {
            string batchFileName = "converter.bat";
            using (StreamWriter sw = new StreamWriter(batchFileName))
            {
                sw.WriteLine(CommandLine);
            }
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + CommandLine);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            Process p = Process.Start(processInfo);
            p.WaitForExit();
        }
    }
}