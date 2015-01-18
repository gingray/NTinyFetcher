using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using xNet.Net;

namespace SandBox
{
    public class MultiThread
    {
        private static object _locker = new object();
        private static StreamWriter _outGoodWriter;
        private static StreamWriter _outBadWriter;
        private static ConcurrentQueue<string> urls;

        static MultiThread()
        {
            _outGoodWriter = new StreamWriter("result.txt");
            _outBadWriter = new StreamWriter("error.txt");
            urls = new ConcurrentQueue<string>(File.ReadAllLines("urls.txt"));
        }

        public void Perform()
        {
            var threads = new Thread[10];
            for (int index = 0; index < threads.Length; index++)
            {
                threads[index] = new Thread(ThreadFunc);
                threads[index].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            _outGoodWriter.Dispose();
            _outBadWriter.Dispose();
        }

        private void ThreadFunc()
        {
            var urlFetcher = new UrlFetcher(GetUrl, OnError, OnAction);
            urlFetcher.Perform();
        }

        private static string GetUrl()
        {
            string item = null;
            var result = urls.TryDequeue(out item);
            return result ? item : null;
        }

        private static void OnError(Exception ex, string url, HttpRequest webClient)
        {
            lock (_locker)
            {
                _outBadWriter.WriteLine(url);
                _outBadWriter.Flush();
            }
        }

        private static void OnAction(string url, HttpRequest webClient)
        {
            var content = webClient.Get(url).ToString().ToLower();
            Console.WriteLine(url);
            lock (_locker)
            {
                _outGoodWriter.WriteLine(url);
                _outGoodWriter.Flush();
            }
        }


    }
}
