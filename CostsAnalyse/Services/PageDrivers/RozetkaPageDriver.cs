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

namespace CostsAnalyse.Services.PageDrivers
{
    public class RozetkaPageDriver:IPageDrivers
    {
        int index = -1;
        List<string> proxyList = new List<string>();
        public RozetkaPageDriver() {
            this.proxyList = ProxyServerConnectionManagment.GetProxyHrefs();
        }
        public HashSet<String> GetPages() {
            HashSet<String> pages ;
            using (FileStream fs = new FileStream("RozetkaHrefs.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                pages = (HashSet<String>)bf.Deserialize(fs);
            }
            return pages;
        }
        public List<Product> GetProducts() {
            var pages = GetPages();
            List<Product> products = new List<Product>();
            foreach (var page in pages) {
                products.Add(ParseProductsFromPage(page));
            }
            return products;
        }
        public List<Product> ParseProductsFromPage(string url)
        {
            
            Random rand = new Random();
            int TimeDelay = 7000 * rand.Next(9);
            Thread.Sleep(TimeDelay);

            

            var products = new List<Product>();
            try
            {
                WebRequest WR = WebRequest.Create(url);
                WR.Method = "GET";
                string[] fulladress = proxyList[index].Split(";");
                var (adress, port) = (fulladress[1],int.Parse(fulladress[2]));
                WebProxy myproxy = new WebProxy(adress, port);
                myproxy.BypassProxyOnLocal = false;
                WR.Proxy = myproxy ;
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
                        
                        var product = rp.GetProduct(urlForPage);
                        if (product != null)
                        {
                            products.Add(product);
                        }
                    }
                    catch (Exception ex)
                    { 
                    }
                }
            }
            catch (TimeoutException ex) {
                if (index < proxyList.Count-1) {
                    return ParseProductsFromPage(url);
                }
            }
            return products; 
        }
 
    }
}
