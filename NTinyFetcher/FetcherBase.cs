using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTinyFetcher
{
    public abstract class FetcherBase<TClient> where TClient : class
    {
        private TClient _webClient;

        public void Perform()
        {
            _webClient = Configure();
            while (true)
            {
                var url = GetUrl();
                if (url == null || Stop)
                    break;
                try
                {
                    BeforeAction(url, _webClient);
                    MakeAction(url, _webClient);
                    AfterAction(url, _webClient);
                }
                catch (Exception ex)
                {
                    ExceptionRecieve(ex,url,_webClient);
                }
            }
            OnFinish();
        }

        protected abstract void MakeAction(string url, TClient webClient);
        protected abstract TClient Configure();
        protected abstract string GetUrl();
        protected abstract void ExceptionRecieve(Exception ex, string url, TClient webClient);

        protected virtual void BeforeAction(string url, TClient webClient){}
        protected virtual void AfterAction(string url, TClient webClient){}
        protected virtual void OnFinish(){}
        protected bool Stop = false;
    }
}
