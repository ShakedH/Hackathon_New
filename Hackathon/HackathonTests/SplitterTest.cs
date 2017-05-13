using Hackathon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace HackathonTests
{
    [TestClass()]
    public class SplitterTest
    {
        [TestMethod()]
        public void SplitTest()
        {

            /*****DAN********
            double interval = 30;
            TimeSpan totalDuration = WavSplitter.GetWavFileDuration("test_Lectur.wav");
            double a = totalDuration.TotalSeconds;
            double timeFromEnd = (totalDuration.TotalSeconds - interval);
            double timeFromStart = 0;
            TimeSpan start = TimeSpan.FromSeconds(timeFromStart);
            TimeSpan end = TimeSpan.FromSeconds(timeFromEnd);
            int i = 1;
            while (end.TotalSeconds > 0)
            {
                WavSplitter.TrimWavFile("test_Lectur.wav", "test_Lectur" + i + ".wav", start, end);
                timeFromEnd -= interval;
                timeFromStart += interval;
                end = TimeSpan.FromSeconds(timeFromEnd);
                start = TimeSpan.FromSeconds(timeFromStart);
                i++;
            }
            */

             WavSplitter.Split(15, "short.wav");

           /* string inputPath = @"C:\Users\Dan gleyzer\Source\Repos\Hackathon_New\Hackathon\HackathonTests\bin\Debug\OutPut3.wav";
            string OutPutPath = @"C:\Users\Dan gleyzer\Source\Repos\Hackathon_New\Hackathon\HackathonTests\bin\Debug\";
            ConvertWavToMono con = new ConvertWavToMono(inputPath, OutPutPath);
            con.Convert();*/
        }

        [TestMethod()]
        public void convertAndSplit()
        {

            string inputPath = @"C:\Users\adamz\Source\Repos\Hackathon_New\Hackathon\HackathonTests\bin\Debug\ted.mp4";
            string OutPutPath = @"C:\Users\adamz\Source\Repos\Hackathon_New\Hackathon\HackathonTests\bin\Debug\";
            VideoToWavConverter vdCconv = new VideoToWavConverter(inputPath, OutPutPath);
            vdCconv.Convert();
            WavSplitter.Split(15, "ted_audio.wav");
        }
    }
}