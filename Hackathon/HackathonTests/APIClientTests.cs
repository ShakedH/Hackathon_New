using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace Hackathon.Tests
{
    [TestClass()]
    public class APIClientTests
    {
        [TestMethod()]
        public void ShortConversionWithTags()
        {
            string file = @"C:\Users\user\Desktop\Hack\s3.wav";
            string response = "";
            try
            {
                APIClient client = new APIClient();
                response = client.Convert(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Fail();
            }
            Assert.IsTrue(response.StartsWith("Right now look fucker"));
            Console.WriteLine(response);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FileNotExist()
        {
            string file = @"C:\Users\user\Desktop\Hack\NotExistFile.wav";
            string response = "";
            APIClient client = new APIClient();
            response = client.Convert(file);
        }

    }
}