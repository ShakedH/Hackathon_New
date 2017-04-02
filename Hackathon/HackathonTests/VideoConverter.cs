using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hackathon;

namespace HackathonTests
{
    [TestClass]
    public class VideoConverter
    {
        [TestMethod]
        public void GoodConversion()
        {
            VideoToWavConverter v = new VideoToWavConverter(@"C:\Users\Ron Michaeli\Downloads\1.mp4", @"C:\Users\Ron Michaeli\Downloads");
            v.Convert();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongInputPath()
        {
            VideoToWavConverter v = new VideoToWavConverter(@"C:\Users\Ron Michaeli\Downloads\9.mp4", @"C:\Users\Ron Michaeli\Downloads");
            v.Convert();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongOutputPath()
        {
            VideoToWavConverter v = new VideoToWavConverter(@"C:\Users\Ron Michaeli\Downloads\1.mp4", @"C:\Users\Ron Michaeli\Downloa");
            v.Convert();
        }
    }
}
