using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTinyFetcher
{
    public interface IWebClient
    {
        string Get(string url);
    }
}
