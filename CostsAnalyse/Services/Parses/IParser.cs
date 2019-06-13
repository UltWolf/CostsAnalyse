using CostsAnalyse.Models;
using System.Collections.Generic;

namespace CostsAnalyse.Services.Parses
{
    public interface IParser
    {
        Product GetProduct(string url,ref List<string> proxyList);
        
    }
}