﻿//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Threading;
//using System.Diagnostics;

//namespace HackathonTests
//{
//    [TestClass()]
//    public class APIClientTests
//    {
//        [TestMethod()]
//        public void ShortConversionWithTags()
//        {
//            string file = @"C:\Users\user\Desktop\Hack\short3.wav";
//            string response = "";
//            try
//            {
//                APIClient client = new APIClient();
//                response = client.Convert(file);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//                Assert.Fail();
//            }

//            Assert.IsTrue(response.StartsWith("Right now look fucker"));
//            Console.WriteLine(response);
//        }


//        [TestMethod()]
//        public void WriteToFileFiveMin()
//        {
//            string inFilePath = @"C:\Users\user\Desktop\Hack\test_Lectur.wav";
//            string outFilePath = @"C:\Users\user\Desktop\Hack\test_Lectur.txt";
//            string response = "";
//            try
//            {
//                APIClient client = new APIClient();
//                response = client.Convert(inFilePath);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//                Assert.Fail();
//            }

//            using (System.IO.StreamWriter outFile =
//          new System.IO.StreamWriter(outFilePath))
//            {
//                outFile.Write(response);
//            }
//            Console.WriteLine(response);
//        }

//        [TestMethod()]
//        [ExpectedException(typeof(ArgumentException))]
//        public void FileNotExist()
//        {
//            string file = @"C:\Users\user\Desktop\Hack\NotExistFile.wav";
//            string response = "";
//            APIClient client = new APIClient();
//            response = client.Convert(file);
//        }

//        [TestMethod()]
//        public void TestMultipleRequestsPerMinutesThreaded()
//        {
//            APIClient client = new APIClient();
//            List<Thread> threads = new List<Thread>();
//            string file = @"C:\Users\user\Desktop\Hack\s3.wav";
//            for (int i = 0; i < 10; i++)
//            {
//                Thread t = new Thread(() =>
//                {
//                    try
//                    {
//                        int j = i;
//                        client.Convert(file, i < 20);
//                        System.Diagnostics.Debug.WriteLine("Converted thread #" + j);
//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine(e.Message);
//                        Assert.Fail();
//                    }
//                });
//                threads.Add(t);
//                t.Start();
//            }

//            int threadsFinished = 0;
//            foreach (Thread t in threads)
//            {
//                t.Join();
//                threadsFinished++;
//            }
//            Console.WriteLine("Threads Finished: " + threadsFinished);

//        }

//        [TestMethod()]
//        public void TestMultipleRequestsPerMinutesNotThreaded()
//        {
//            Stopwatch sw = new Stopwatch();
//            APIClient client = new APIClient();
//            string fileStart = @"C:\Users\user\Desktop\Hack\";
//            sw.Start();
//            int i;
//            for (i = 0; i < 3; i++)
//            {

//                string file = fileStart + "test_Lectur" + (i + 1) + ".wav";
//                try
//                {
//                    int j = i;
//                    string response = client.Convert(file, i < 20);
//                    using (System.IO.StreamWriter outFile =
//          new System.IO.StreamWriter(fileStart + i + ".txt"))
//                    {
//                        outFile.Write(response);
//                    }
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine(e.Message);
//                    Assert.Fail();
//                }
//            }
//            sw.Stop();
//            string time = sw.Elapsed.TotalSeconds.ToString();
//            Console.WriteLine(string.Format("converted {0} requests in {1} seconds", i, time));

//        }
//    }
//}