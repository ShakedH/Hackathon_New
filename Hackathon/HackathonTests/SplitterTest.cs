using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace Hackathon.Tests
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

            WavSplitter.Split(30, "test_Lecture");


        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FileNotExist()
        {

        }






    }
}