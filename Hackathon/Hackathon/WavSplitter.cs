using NAudio.Wave;
using System;
using System.IO;

namespace Hackathon
{

    public static class WavSplitter
    {
        public static void TrimWavFile(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
        {
            using (WaveFileReader reader = new WaveFileReader(inPath))
            {
                using (WaveFileWriter writer = new WaveFileWriter(outPath, new WaveFormat()))
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
                   // int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {

                        writer.Write(buffer, 0, bytesRead);
                    }
                    else if (bytesRead == 0)
                    {
                        break;
                    }
                }
                else if (bytesRequired <= 0)
                {
                    break;
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

            string outPutPath = Path.GetFullPath((Path.Combine(Path.Combine(path, @"..\"))));

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
                string outPutfilePath = outPutPath + "OutPut" + i + ".wav";
                WavSplitter.TrimWavFile(path, outPutfilePath, start, end);
                timeFromEnd -= interval;
                timeFromStart += interval;
                end = TimeSpan.FromSeconds(timeFromEnd);
                start = TimeSpan.FromSeconds(timeFromStart);
                i++;
                //convert to flac
                string inputPath = outPutfilePath;
                string OutPutPath = @".\\";
                ConvertWavToMono con = new ConvertWavToMono(inputPath, OutPutPath);
                con.Convert();
                File.Delete(outPutfilePath);
            }


        }



    }
}

