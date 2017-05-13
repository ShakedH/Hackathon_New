using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Hackathon;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HackathonTests
{
    [TestClass]
    public class MainIndexTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<string, List<VideoDetails>> dic = new Dictionary<string, List<VideoDetails>>();
            List<string> keywords = new List<string>();
            keywords.Add("communication");
            keywords.Add("network");
            keywords.Add("transport");
            keywords.Add("application");
            keywords.Add("internet");
            VideoDetails vd1 = new VideoDetails("Communication", keywords);
            List<VideoDetails> vdList = new List<VideoDetails>();
            vdList.Add(vd1);
            dic["communication"] = vdList;
            dic["network"] = vdList;
            dic["transport"] = vdList;
            dic["internet"] = vdList;
            dic["application"] = vdList;
            dic["layers"] = vdList;
            dic["layer"] = vdList;
            dic["link"] = vdList;
            dic["data"] = vdList;
            dic["address"] = vdList;

            using (FileStream stream = File.Open("MainIndex.bin", FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, dic);
            }
        }
    }
}