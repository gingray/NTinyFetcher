using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTinyFetcher
{
    public abstract class FetcherBase<TClient> where TClient : class, IWebClient
    {
        private readonly TClient _webClient;

        protected FetcherBase(TClient webClient)
        {
            _webClient = webClient;
        }

        public void Perform()
        {
            Configure(_webClient);
            while (true)
            {
                var url = GetUrl();
                if(url == null)
                    break;
                try
                {
                    MakeAction(url, _webClient);
                }
                catch (Exception ex)
                {
                    ExceptionRecieve(ex);
                }
            }
        }

        protected abstract void MakeAction(string url, TClient webClient);
        protected abstract void Configure(TClient webClient);
        protected abstract string GetUrl();
        protected abstract void ExceptionRecieve(Exception ex);
    }
}
