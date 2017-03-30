using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hackathon;

namespace HackathonTests
{
    [TestClass]
    public class VideoConverter
    {
        [TestMethod]
        public void TestConvert()
        {
            VideoToWavConverter v = new VideoToWavConverter(@"C:\Users\Ron Michaeli\Downloads\1.mp4", @"C:\Users\Ron Michaeli\Downloads");
            v.Convert();
        }
    }
}
