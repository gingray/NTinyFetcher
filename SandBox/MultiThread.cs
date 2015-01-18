using System;
using System.Collections.Concurrent;
using System.IO;
using NTinyFetcher;
using xNet.Net;

namespace SandBox
{
    public class MultiThread : MultiFetcher
    {
        private static readonly object _locker = new object();
        private readonly StreamWriter _outGoodWriter;
        private readonly StreamWriter _outBadWriter;
        private readonly ConcurrentQueue<string> _urls;

        public MultiThread(int threadCount)
            : base(threadCount)
        {
            _outGoodWriter = new StreamWriter("result.txt");
            _outBadWriter = new StreamWriter("error.txt");
            _urls = new ConcurrentQueue<string>(File.ReadAllLines("urls.txt"));
        }

        protected override void OnFinish()
        {
            _outGoodWriter.Dispose();
            _outBadWriter.Dispose();
        }

        protected override void ThreadFunc()
        {
            var urlFetcher = new UrlFetcher(GetUrl, OnError, OnAction);
            urlFetcher.Perform();
        }

        private string GetUrl()
        {
            string item = null;
            var result = _urls.TryDequeue(out item);
            return result ? item : null;
        }

        private void OnError(Exception ex, string url, HttpRequest webClient)
        {
            lock (_locker)
            {
                _outBadWriter.WriteLine(url);
                _outBadWriter.Flush();
            }
        }

        private void OnAction(string url, HttpRequest webClient)
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
