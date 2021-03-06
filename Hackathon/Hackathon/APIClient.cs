﻿/**
Copyright (c) Microsoft Corporation
All rights reserved. 
MIT License
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
**/
using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Hackathon
{
    public class APIClient
    {

        private const string URL = "https://speech.platform.bing.com/recognize";
        private const string AuthenticationKey1 = "983ad0ecbb4d40db8bf6fac8c88022d0";
        private const string AuthenticationKey2 = "89d52a07c0cb45a9ac03a719fe2f6950";


        public string Convert(string FilePath, bool firstKey = true)
        {
            if (!File.Exists(FilePath))
                throw new ArgumentException(string.Format("File {0} not found", FilePath));

            // Note: Sign up at http://www.projectoxford.ai to get a subscription key.  Search for Speech APIs from Azure Marketplace.  
            // Use the subscription key as Client secret below.
            Authentication auth;
            if (firstKey)
                auth = new Authentication(AuthenticationKey1);
            else
                auth = new Authentication(AuthenticationKey2);



            string requestUri = URL.Trim(new char[] { '/', '?' });

            /* URI Params. Refer to the README file for more information. */
            requestUri += @"?scenarios=smd";                                  // websearch is the other main option.
            requestUri += @"&appid=D4D52672-91D7-4C74-8AD8-42B1D98141A5";     // You must use this ID.
            requestUri += @"&locale=en-US";                                   // We support several other languages.  Refer to README file.
            requestUri += @"&device.os=wp7";
            requestUri += @"&version=3.0";
            requestUri += @"&format=json";
            requestUri += @"&instanceid=565D69FF-E928-4B7E-87DA-9A750B96D9E3";
            requestUri += @"&requestid=" + Guid.NewGuid().ToString();

            string host = @"speech.platform.bing.com";
            string contentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";

            /*
             * Input your own audio file or use read from a microphone stream directly.
             */
            string responseString;
            FileStream fs = null;

            var token = auth.GetAccessToken();

            HttpWebRequest request = null;
            request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.SendChunked = true;
            request.Accept = @"application/json;text/xml";
            request.Method = "POST";
            request.ProtocolVersion = HttpVersion.Version11;
            request.Host = host;
            request.ContentType = contentType;
            request.Headers["Authorization"] = "Bearer " + token;

            using (fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {

                /*
                 * Open a request stream and write 1024 byte chunks in the stream one at a time.
                 */
                byte[] buffer = null;
                int bytesRead = 0;
                using (Stream requestStream = request.GetRequestStream())
                {
                    /*
                     * Read 1024 raw bytes from the input audio file.
                     */
                    buffer = new Byte[checked((uint)Math.Min(1024, (int)fs.Length))];
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }

                    // Flush
                    requestStream.Flush();
                }

                /*
                 * Get the response from the service.
                 */
                using (WebResponse response = request.GetResponse())
                {
                    Console.WriteLine(((HttpWebResponse)response).StatusCode);

                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseString = sr.ReadToEnd();
                    }
                }
            }
            return ProcessResponse(responseString);
        }

        private string ProcessResponse(string responseString)
        {
            JObject json = JObject.Parse(responseString);
            string status = json["header"]["status"].Value<string>();
            if (status != "success")
                throw new IOException(string.Format("Failed retrieving data from server. status: {0} ", status));
            string text = json["header"]["name"].Value<string>();
            text = text.Trim(new char[] { '<', '>' });
            text = Regex.Replace(text, "<.*?>", string.Empty);
            return text;
        }
    }

    /*
    * This class demonstrates how to get a valid O-auth token.
    */
    class Authentication
    {
        public static readonly string FetchTokenUri = "https://api.cognitive.microsoft.com/sts/v1.0";
        private string subscriptionKey;
        private string token;
        private Timer accessTokenRenewer;

        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;

        public Authentication(string subscriptionKey)
        {
            this.subscriptionKey = subscriptionKey;
            this.token = FetchToken(FetchTokenUri, subscriptionKey).Result;

            // renew the token every specfied minutes
            accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback),
                                           this,
                                           TimeSpan.FromMinutes(RefreshTokenDuration),
                                           TimeSpan.FromMilliseconds(-1));
        }

        public string GetAccessToken()
        {
            return this.token;
        }

        private void RenewAccessToken()
        {
            this.token = FetchToken(FetchTokenUri, this.subscriptionKey).Result;
            Console.WriteLine("Renewed token.");
        }

        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed renewing access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
        }

        private async Task<string> FetchToken(string fetchUri, string subscriptionKey)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                UriBuilder uriBuilder = new UriBuilder(fetchUri);
                uriBuilder.Path += "/issueToken";

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }

}
