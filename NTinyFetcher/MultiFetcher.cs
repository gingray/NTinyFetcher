using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NTinyFetcher
{
    public abstract class MultiFetcher
    {
        private readonly int _threadCount;

        protected MultiFetcher(int threadCount)
        {
            _threadCount = threadCount;
        }

        protected abstract void ThreadFunc();
        protected abstract void OnFinish();

        public void Perform()
        {
            var threads = new Thread[_threadCount];
            for (int i = 0; i < _threadCount; i++)
            {
                threads[i] = new Thread(ThreadFunc);
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            OnFinish();
        }
    }
}
