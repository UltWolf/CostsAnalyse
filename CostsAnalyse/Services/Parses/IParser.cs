using CostsAnalyse.Models;

namespace CostsAnalyse.Services.Parses
{
    public interface IParser
    {
        Product GetProduct(string url);
        
    }
}