using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using CostsAnalyse.Extensions;
using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.Parses;
using CostsAnalyse.Services.ProxyServer;
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
        private readonly ApplicationContext _context;
        private List<string> proxyList;
        public FoxtrotPageDriver() {
            this.proxyList = ProxyServerConnectionManagment.GetProxyHrefs();
            GenerateHrefs();

        }
        public FoxtrotPageDriver(ApplicationContext context) {
            this.proxyList = ProxyServerConnectionManagment.GetProxyHrefs();
            GenerateHrefs();
            this._context = context;
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

            ThreadDelay.Delay();

            try
            {
                WebRequest WR = WebRequest.Create(url);
                WR.Method = "GET";
                //string[] fulladress = proxyList[++index].Split(":");
                //var (adress, port) = (fulladress[0], int.Parse(fulladress[1]));
                //WebProxy myproxy = new WebProxy(adress, port);
                //myproxy.BypassProxyOnLocal = false;
                //WR.Proxy = myproxy;
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
                    if (_context != null)
                    {
                        AddToDBContext(product);
                    }
                }
            }
            catch (Exception ex) when (ex.Message == "Подключение не установлено, т.к.конечный компьютер отверг запрос на подключение Подключение не установлено, т.к.конечный компьютер отверг запрос на подключение")
            {

                if (proxyList.Count - 1 > index)
                {
                    proxyList.Remove(proxyList[index]);
                }
                ParseProductsFromPage(url, index);
            }
            catch (Exception ex)
            {

                if (index < proxyList.Count - 1)
                {
                    index++;
                    ParseProductsFromPage(url, index++);
                }
            }
            ProxyServerConnectionManagment.SerializeByPuts(proxyList);
        }
        private Product CreateInstanseOfProduct(IElement div,string url)
        {
            try
            {
                FoxtrotParser fp = new FoxtrotParser();
                string urlForPage = div.GetElementsByClassName("detail-link")[0].GetAttribute("href");
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
                var productFromContext = _context.Products.FirstOrDefault(m => m.Index == product.Index);
                var currentCost = product.LastPrice[0].Cost;
                if (productFromContext == null)
                {
                    product.Price = product.LastPrice;
                    product.Min = currentCost;
                    product.Max = currentCost;
                    _context.Add(product);
                }
                else
                {
                    productFromContext.Price.Add(product.LastPrice);
                    var lastPrice = productFromContext.LastPrice.FirstOrDefault(m => m.Company.Equals(product.LastPrice[0].Company));
                    if (lastPrice != null)
                    {
                        lastPrice = product.LastPrice[0];
                    }
                    else
                    {
                        productFromContext.LastPrice.Add(product.LastPrice[0]);
                    }
                    if (currentCost > productFromContext.Max)
                    {
                        productFromContext.Max = currentCost;
                    }
                    else if (currentCost < productFromContext.Min)
                    {
                        productFromContext.Min = currentCost;
                    }
                    _context.Products.Update(productFromContext);
                }

                _context.SaveChanges();
            }
    }
        private int getPriceFromNumb(IElement element)
        {
            return int.Parse(element.GetElementsByClassName("numb")[0].TextContent.Replace(" ", ""));
        }

        public void GenerateHrefs()
        {
            if (!File.Exists("FoxtrotHrefs.txt")) {
                RozetkaMenuDriver RMD = new RozetkaMenuDriver();
                RMD.GetPagesAuto();
            }
        }
    }
}
