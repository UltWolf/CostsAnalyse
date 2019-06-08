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
        void ParseProductsFromPage(string url,int index);
        void GetProducts();


    }
}
