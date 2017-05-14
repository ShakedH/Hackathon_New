using Hackathon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace HackathonTests
{
    [TestClass()]
    public class ProgramTests
    {

        string videoFile = @"C:\Users\Dan gleyzer\Source\Repos\Hackathon_New\Hackathon\HackathonTests\bin\Debug\ClimateChange.mp4";
        string binaryDirectory = @"C:\Users\Dan gleyzer\Source\Repos\Hackathon_New\Hackathon\HackathonTests\bin\Debug\";
        string videoName = "ClimateChange";
        Stopwatch sw = new Stopwatch();
        Converter converter = new Converter(new APIGoogleClient());


        [TestMethod()]
        public void ConvertVideoTest()
        {
            sw.Restart();
            try
            {
                Video vid = converter.ConvertVideo(videoFile, videoName);
                vid.SaveToFile(binaryDirectory);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Fail();
            }
            sw.Stop();
            Console.WriteLine("Finished in {0} seconds", sw.Elapsed.TotalSeconds);
            Assert.IsTrue(true);
        }
    }
}

//        [TestMethod()]
//        public void TestLoad()
//        {
//            Converter program = new Converter(new APIGoogleClient());
//            try
//            {
//                program.LoadFromFile(binaryDirectory);
//                Dictionary<string, List<TimeInVid>> dic = program.Terms;
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//                Assert.Fail();
//            }
//            Assert.IsTrue(true);
//        }


//        [TestMethod()]
//        public void TestMostFrequent()
//        {
//            program.LoadFromFile(binaryDirectory);
//            Dictionary<string, List<TimeInVid>> max = program.GetMostFrequentStrings(30);
//        }

//        [TestMethod()]
//        public void TestGet()
//        {
//            program.LoadFromFile(binaryDirectory);
//            List<TimeInVid> times = program.SearchWord("layer");
//        }

//        [TestMethod()]
//        public void TestGetSentence()
//        {
//            program.LoadFromFile(binaryDirectory);
//            List<TimeInVid> times = program.SearchWord("layer");
//            Console.WriteLine(program.GetSentence("layer", times[0]));
//        }
//    }
//}
