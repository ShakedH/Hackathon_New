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
            Dictionary<string, List<VideoDetails>> mainIndex = new Dictionary<string, List<VideoDetails>>();
            //byte[] index = Hackathon.Properties.Resources.MainIndex;
            //using (Stream stream = new MemoryStream(index))
            //{
            //    BinaryFormatter reader = new BinaryFormatter();
            //    mainIndex = (Dictionary<string, List<VideoDetails>>)reader.Deserialize(stream);
            //}
            string fileName = "Communication";
            Video video = Video.LoadVideoFromResource(fileName);
            video.Metadata = new VideoMetadata("Engineering", "Information Sys. Engineering", "Communication 101", "Dr. Gal Shpitz");
            List<string> keywords = new List<string>(video.GetMostFrequentStrings(5).Keys);
            VideoDetails vd = new VideoDetails(fileName, keywords);
            foreach (string term in video.Terms.Keys)
            {
                if (!mainIndex.ContainsKey(term))
                    mainIndex[term] = new List<VideoDetails>();
                mainIndex[term].Add(vd);
            }
            using (FileStream stream = File.Open("MainIndex.bin", FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, mainIndex);
            }

            using (FileStream stream = File.Open("CommunicationFull.bin", FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, video);
            }

            //Dictionary<string, List<VideoDetails>> dic = new Dictionary<string, List<VideoDetails>>();
            //List<string> keywords = new List<string>();
            //keywords.Add("communication");
            //keywords.Add("network");
            //keywords.Add("transport");
            //keywords.Add("application");
            //keywords.Add("internet");
            //VideoDetails vd1 = new VideoDetails("Communication", keywords);
            //List<VideoDetails> vdList = new List<VideoDetails>();
            //vdList.Add(vd1);
            //dic["communication"] = vdList;
            //dic["network"] = vdList;
            //dic["transport"] = vdList;
            //dic["internet"] = vdList;
            //dic["application"] = vdList;
            //dic["layers"] = vdList;
            //dic["layer"] = vdList;
            //dic["link"] = vdList;
            //dic["data"] = vdList;
            //dic["address"] = vdList;

            //using (FileStream stream = File.Open("MainIndex.bin", FileMode.Create))
            //{
            //    BinaryFormatter writer = new BinaryFormatter();
            //    writer.Serialize(stream, dic);
            //}
        }

        [TestMethod]
        public void UpdateIndexWithNewVideo()
        {
            Dictionary<string, List<VideoDetails>> mainIndex;
            byte[] index = Hackathon.Properties.Resources.MainIndex;
            using (Stream stream = new MemoryStream(index))
            {
                BinaryFormatter reader = new BinaryFormatter();
                mainIndex = (Dictionary<string, List<VideoDetails>>)reader.Deserialize(stream);
            }

            string fileName = "LieDetection";
            Video video = Video.LoadVideoFromResource(fileName);
            video.Metadata = new VideoMetadata("Humanities & Social Science", "Psychology", "Intro to Lie Detection", "Dr. Zehava Shemesh");
            List<string> keywords = new List<string>(video.GetMostFrequentStrings(5).Keys);
            VideoDetails vd = new VideoDetails(fileName, keywords);
            foreach (string term in video.Terms.Keys)
            {
                if (!mainIndex.ContainsKey(term))
                    mainIndex[term] = new List<VideoDetails>();
                mainIndex[term].Add(vd);
            }
            using (FileStream stream = File.Open("MainIndex.bin", FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, mainIndex);
            }

            using (FileStream stream = File.Open("LieDetectionFull.bin", FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, video);
            }
        }
    }
}