using NAudio.Wave;
using System;

namespace Hackathon
{

    public static class WavSplitter
    {
        public static void TrimWavFile(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
        {
            using (WaveFileReader reader = new WaveFileReader(inPath))
            {
                using (WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat))
                {
                    int bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000;

                    int startPos = (int)cutFromStart.TotalMilliseconds * bytesPerMillisecond;
                    startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

                    int endBytes = (int)cutFromEnd.TotalMilliseconds * bytesPerMillisecond;
                    endBytes = endBytes - endBytes % reader.WaveFormat.BlockAlign;
                    int endPos = (int)reader.Length - endBytes;

                    TrimWavFile(reader, writer, startPos, endPos);
                }
            }
        }

        private static void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
        {
            reader.Position = startPos;
            byte[] buffer = new byte[1024];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        public static TimeSpan GetWavFileDuration(string fileName)
        {
            WaveFileReader wf = new WaveFileReader(fileName);
            return wf.TotalTime;
        }

        public static void Split(double myIntervalInSec, string path)
        {

            double interval = myIntervalInSec;
            TimeSpan totalDuration = WavSplitter.GetWavFileDuration(path);
            double a = totalDuration.TotalSeconds;
            double timeFromEnd = (totalDuration.TotalSeconds - interval);
            double timeFromStart = 0;
            TimeSpan start = TimeSpan.FromSeconds(timeFromStart);
            TimeSpan end = TimeSpan.FromSeconds(timeFromEnd);
            int i = 1;
            while (end.TotalSeconds > 0)
            {
                WavSplitter.TrimWavFile(path, "OutPut" + i + ".wav", start, end);
                timeFromEnd -= interval;
                timeFromStart += interval;
                end = TimeSpan.FromSeconds(timeFromEnd);
                start = TimeSpan.FromSeconds(timeFromStart);
                i++;
            }

        }
    }
}

