using CostsAnalyse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.PageDrivers
{
    public interface IPageDrivers
    {
        HashSet<String> GetPages();
        List<Product> ParseProductsFromPage(string url);
        List<Product> GetProducts();


    }
}
