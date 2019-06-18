using AngleSharp.Html.Parser;
using CostsAnalyse.Extensions;
using CostsAnalyse.Models;
using CostsAnalyse.Services.Parses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using CostsAnalyse.Services.ProxyServer;
using CostsAnalyse.Models.Context;

namespace CostsAnalyse.Services.PageDrivers
{
    public class RozetkaPageDriver : IPageDrivers
    {
        List<string> proxyList = new List<string>();
        private readonly ApplicationContext _context;
        private readonly Repositories.ProductRepository _productRepository;
        public RozetkaPageDriver(ApplicationContext Context)
        {
            this.proxyList = ProxyServerConnectionManagment.GetProxyHrefs();
            this._context = Context;
            this._productRepository = new Repositories.ProductRepository(_context);
            GenerateHrefs();
        }
        public HashSet<String> GetPages()
        {
            HashSet<String> pages;
            using (FileStream fs = new FileStream("RozetkaHrefs.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                pages = (HashSet<String>)bf.Deserialize(fs);
            }
            return pages;
        }
        public void GetProducts()
        {

            var pages = GetPages();
            List<Product> products = new List<Product>();
            foreach (var page in pages)
            {
                if (proxyList.Count <= 1)
                {
                    break;
                }
                ParseProductsFromPage(page, 0);
            }

        }
        public void ParseProductsFromPage(string url, int index)
        {
            string baseUrl = url;
            if (index != 0)
            {
                url += "/page=" + index + "/";
            }
            ThreadDelay.Delay();

            try
            {
                WebRequest WR = WebRequest.Create(url);
                WR.Method = "GET";
                string[] fulladress = proxyList[++index].Split(":");
                var (adress, port) = (fulladress[0], int.Parse(fulladress[1]));
                WebProxy myproxy = new WebProxy(adress, port);
                myproxy.BypassProxyOnLocal = false;
                WR.Proxy = myproxy;
                WebResponse response = WR.GetResponse();
                string html;
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                }

                HtmlParser parser = new HtmlParser();
                var parseElement = parser.ParseDocument(html);
                var divsWithProduct = parseElement.GetElementsByClassName("g-i-tile-catalog");
                
                RozetkaParser rp = new RozetkaParser();
                foreach (var div in divsWithProduct)
                {
                    try
                    {
                        string urlForPage = div.GetElementsByClassName("g-i-tile-i-title")[0].GetElementsByTagName("a")[0].GetAttribute("href");
                        var product = rp.GetProduct(urlForPage, ref proxyList);
                        _productRepository.AddProduct(product);

                    }
                    catch (Exception ex)
                    {
                    }
                }
                var paginator = parseElement.GetElementsByClassName("paginator-catalog");
                string page = "";
                if (paginator.Length > 0)
                {
                    var lis = paginator[0].GetElementsByTagName("li");
                    for (int i = 0; i < lis.Length; i++)
                    {
                        if (lis[i].ClassList.Contains("active"))
                        {
                            page = lis[++i].GetElementsByTagName("span")[0].TextContent;
                            break;
                        }
                    }
                }
                if (page != "")
                {
                    ParseProductsFromPage(baseUrl, int.Parse(page));
                }
            }
            
            catch (Exception ex)
            {
 
            } 
        }

        public void GenerateHrefs()
        {
            if (!File.Exists("RozetkaHrefs.txt"))
            {
                RozetkaMenuDriver RMD = new RozetkaMenuDriver();
                RMD.GetPagesAuto();
            }
        }
    }
}
