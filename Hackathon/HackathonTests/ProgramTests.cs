using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hackathon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hackathon.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        string videoFile = @"C:\Users\user\Desktop\Hack\20min.mp4";
        string binaryFile = @"C:\Users\user\Desktop\Hack\20min.bin";
        Stopwatch sw = new Stopwatch();
        Program program = new Program(new APIClient());

        [TestMethod()]
        public void ConvertVideoTest()
        {
            sw.Restart();
            try
            {
                Dictionary<string, List<TimeInVid>> terms = program.ConvertVideo(videoFile);
                program.SaveToFile(binaryFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                program.SaveToFile(binaryFile);
                Console.WriteLine("Parsed {0} files", program.FilesParsed);
                Assert.Fail();
            }
            sw.Stop();
            Console.WriteLine("Finished in {0} seconds", sw.Elapsed.TotalSeconds);
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void TestLoad()
        {
            Program program = new Program(new APIClient());
            try
            {
                program.LoadFromFile(binaryFile);
                Dictionary<string, List<TimeInVid>> dic = program.Terms;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }


        [TestMethod()]
        public void TestMostFrequent()
        {
            program.LoadFromFile(binaryFile);
            Dictionary<string, List<TimeInVid>> max = program.GetMostFrequentStrings(30);
        }

        [TestMethod()]
        public void TestGet()
        {
            program.LoadFromFile(binaryFile);
            List<TimeInVid> times = program.SearchWord("layer");
        }
    }
}
