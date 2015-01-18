using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using NTinyFetcher;
using xNet.Net;

namespace SandBox
{
    public class UrlFetcher : FetcherBase<HttpRequest>
    {
        private readonly Func<string> _getUrl;
        private readonly Action<Exception, string, HttpRequest> _onError;
        private readonly Action<string, HttpRequest> _onAction;

        public UrlFetcher(Func<string> getUrl, Action<Exception,string,HttpRequest> onError,Action<string,HttpRequest> onAction)
        {
            _getUrl = getUrl;
            _onError = onError;
            _onAction = onAction;
        }

        protected override void MakeAction(string url, HttpRequest webClient)
        {
            _onAction(url, webClient);
        }

        protected override HttpRequest Configure()
        {
            return new HttpRequest {ConnectTimeout = 5000, ReadWriteTimeout = 5000};
        }

        protected override string GetUrl()
        {
            return _getUrl();
        }

        protected override void ExceptionRecieve(Exception ex,string url,HttpRequest webClient)
        {
            _onError(ex, url, webClient);
        }
    }
}
