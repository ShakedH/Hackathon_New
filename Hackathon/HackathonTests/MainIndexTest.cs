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

        [TestMethod]
        public void AddEmptyStringToMainIndex()
        {
            Dictionary<string, List<VideoDetails>> mainIndex;
            byte[] index = Hackathon.Properties.Resources.MainIndex;
            using (Stream stream = new MemoryStream(index))
            {
                BinaryFormatter reader = new BinaryFormatter();
                mainIndex = (Dictionary<string, List<VideoDetails>>)reader.Deserialize(stream);
            }

            mainIndex[""] = new List<VideoDetails>();

            string fileName1 = "LieDetection";
            Video video1 = Video.LoadVideoFromResource(fileName1);
            List<string> keywords1 = new List<string>(video1.GetMostFrequentStrings(5).Keys);
            VideoDetails vd1 = new VideoDetails(fileName1, keywords1);

            string fileName2 = "Communication";
            Video video2 = Video.LoadVideoFromResource(fileName2);
            List<string> keywords2 = new List<string>(video2.GetMostFrequentStrings(5).Keys);
            VideoDetails vd2 = new VideoDetails(fileName2, keywords2);

            string fileName3 = "ClimateChange";
            Video video3 = Video.LoadVideoFromResource(fileName3);
            List<string> keywords3 = new List<string>(video3.GetMostFrequentStrings(5).Keys);
            VideoDetails vd3 = new VideoDetails(fileName3, keywords3);

            mainIndex[""].Add(vd1);
            mainIndex[""].Add(vd2);
            mainIndex[""].Add(vd3);

            using (FileStream stream = File.Open("MainIndex.bin", FileMode.Create))
            {
                BinaryFormatter writer = new BinaryFormatter();
                writer.Serialize(stream, mainIndex);
            }
        }
    }
}