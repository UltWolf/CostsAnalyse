using CostsAnalyse.Models;
using System.Collections.Generic;

namespace CostsAnalyse.Services.Parses
{
    public interface IParser
    {
        Product GetProduct(string url,int index,ref List<string> proxyList);
        
    }
}