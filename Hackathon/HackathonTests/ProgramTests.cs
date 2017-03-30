using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hackathon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void ConvertVideoTest()
        {
            string videoFile = @"C:\Users\user\Desktop\Hack\1.mp4";
            string audioFile = @"C:\Users\user\Desktop\Hack\1.wav";
            string binaryFile = @"C:\Users\user\Desktop\Hack\1.bin";
            Program program = new Program(new APIClient(), new VideoToWavConverter(videoFile, audioFile));
            try
            {
                Dictionary<Term, List<TimeInVid>> terms = program.ConvertVideo(audioFile);
                program.SaveToFile(binaryFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Fail();
            }
        }
    }
}