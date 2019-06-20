using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using CostsAnalyse.Extensions;
using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.MenuDrivers;
using CostsAnalyse.Services.Parses;
using CostsAnalyse.Services.ProxyServer;
using CostsAnalyse.Services.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.PageDrivers
{
    public class FoxtrotPageDriver : IPageDrivers
    {
        private readonly BinaryFormatter _bf = new BinaryFormatter();
        private readonly Logging.FileLogging fl = new Logging.FileLogging();
        private readonly ApplicationContext _context;
        private readonly ProductRepository _productRepository;
        private List<string> proxyList;
         
        public FoxtrotPageDriver(ApplicationContext context) {
            this.proxyList = ProxyServerConnectionManagment.GetProxyHrefs();
            GenerateHrefs();
            this._context = context;
            _productRepository = new ProductRepository(_context);
        }
        

        public HashSet<string> GetPages()
        {
            HashSet<string> hrefs;
            using (FileStream fs = new FileStream("FoxtrotHrefs.txt", FileMode.Open, FileAccess.Read))
            {
                hrefs = (HashSet<String>)_bf.Deserialize(fs);
            }
            return hrefs;
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
            if (index != 0)
            {
                url += "?page=" + index;
            }
            ThreadDelay.Delay();
            foreach (var proxy in proxyList)
            {
                try
                {
                    string baseUrl = url;
                    WebRequest WR;
                   
                    if (url.Contains("https://www.foxtrot.com.ua"))
                    {
                        WR = WebRequest.Create(url);
                    }
                    else
                    {
                      WR   = WebRequest.Create("https://www.foxtrot.com.ua" + url);
                    }
                    WR.Method = "GET";
                    WR.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:65.0) Gecko/20100101 Firefox/65.0");
                    string[] fulladress = proxy.Split(":");
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
                    var divsWithProduct = parseElement.GetElementsByClassName("product-listing")[0]
                                                      .GetElementsByClassName("product-item");
                  

                    foreach (var div in divsWithProduct)
                    {
                        ThreadDelay.Delay();
                        var product = CreateInstanseOfProduct(div, url);
                        if (product != null)
                        {
                            if (_context != null)
                            {
                                AddToDBContext(product);
                            }
                        }
                    }
                    string page = "";
                    var pagination = parseElement.GetElementsByClassName("pagination-number-list");
                    if (pagination.Length > 0)
                    {
                        var pages = pagination[0].GetElementsByTagName("li");
                        for (int i = 0; i < pages.Length; i++)
                        {
                            if (pages[i].ClassList.Contains("active"))
                            {
                                page = pages[++i].GetElementsByTagName("a")[0].GetElementsByTagName("span")[0].TextContent;
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
                    fl.LogAsync(ex, new List<string>() { proxy, url });
                    
                }
            }
        }
        private Product CreateInstanseOfProduct(IElement div,string url)
        {
            try
            {
                FoxtrotParser fp = new FoxtrotParser();
                var elementDetailLink = div.GetElementsByClassName("detail-link")[0];
                string urlForPage = elementDetailLink.GetAttribute("href");
                var img = elementDetailLink.GetElementsByTagName("img")[0];
                var product = fp.GetProduct(urlForPage,  ref proxyList);

                product.Name = div.GetAttribute("data-title");
                product.Index = product.Name.Split("(")[1].Split(")")[0];
                
                var price = div.GetElementsByClassName("price")[0];
                var priceWrapper = price.GetElementsByClassName("price__wrapper")[0];
                var price_relevant = priceWrapper.GetElementsByClassName("price__relevant")[0];
                decimal currentPrice = getPriceFromNumb(price_relevant);
                var discontWrapper = price.GetElementsByClassName("price__not-relevant");

                if (discontWrapper.Length > 0)
                {
                    decimal oldPrice = getPriceFromNumb(price_relevant);
                    product.LastPrice = new List<Price>() { new Price(currentPrice,oldPrice,
                                new Company("Foxtrot", url)) { IsDiscont=true,Discont =  Math.Truncate(
                                                                                   (1-
                                                                                   (currentPrice/oldPrice))
                                                                                                     *100)} };
                }
                else
                {

                    product.LastPrice = new List<Price>() { new Price(
                    currentPrice,
                    new Company("Foxtrot", url))
                    { IsDiscont=false} };
                }
                return product;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
        public void AddToDBContext(Product product)
        {
            if (!product.IsNull())
            {
  
                    _productRepository.AddProduct(product); 
            }
    }
        private int getPriceFromNumb(IElement element)
        {
            return int.Parse(element.GetElementsByClassName("numb")[0].TextContent.Replace(" ", ""));
        }

        public void GenerateHrefs()
        {
            if (!File.Exists("FoxtrotHrefs.txt")) {
                FoxtrotMenuDriver RMD = new FoxtrotMenuDriver();
                RMD.GetPagesAuto();
            }
        }
    }
}
