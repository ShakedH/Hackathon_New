using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hackathon;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace HackathonTests
{
    [TestClass]
    public class VideoTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Video v = Video.LoadVideoFromFile(Directory.GetCurrentDirectory() + @"\Communication.bin");
            v.GetMostFrequentStrings(10);
            v.SaveToFile(Directory.GetCurrentDirectory());
        }

        [TestMethod]
        public void TestMethod2()
        {
            string path = Directory.GetCurrentDirectory() + @"\Communication.bin";
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter reader = new BinaryFormatter();
                Video v = (Video)reader.Deserialize(stream);
            }
        }
    }
}
