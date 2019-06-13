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
    public class RozetkaPageDriver:IPageDrivers
    {
        List<string> proxyList = new List<string>();
        private readonly ApplicationContext _context;
        public RozetkaPageDriver(ApplicationContext Context) {
            this.proxyList = ProxyServerConnectionManagment.GetProxyHrefs();
            this._context = Context;
            GenerateHrefs();
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
        public void GetProducts() {
            
            var pages = GetPages();
            List<Product> products = new List<Product>();
            foreach (var page in pages)
            {
                if (proxyList.Count <= 1)
                {
                        break;
                }
               ParseProductsFromPage(page,0);
            }
           
        }
        public void ParseProductsFromPage(string url,int index)
        {
            
            ThreadDelay.Delay();
           
            try
            {
                WebRequest WR = WebRequest.Create(url);
                WR.Method = "GET";
                string[] fulladress = proxyList[++index].Split(":");
                var (adress, port) = (fulladress[0],int.Parse(fulladress[1]));
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
                        var product = rp.GetProduct(urlForPage,ref proxyList);
                        if(!product.IsNull()){
                        var productFromContext = _context.Products.FirstOrDefault(m=> m.Index == product.Index);
                        var currentCost = product.LastPrice[0].Cost;
                        if(productFromContext ==null){
                            product.Price = product.LastPrice;
                            product.Min = currentCost;
                            product.Max = currentCost;
                            _context.Add(product);
                        }else{
                            productFromContext.Price.Add(product.LastPrice);
                            var  lastPrice = productFromContext.LastPrice.FirstOrDefault(m=>m.Company.Equals(product.LastPrice[0].Company));
                            if(lastPrice!=null){
                                lastPrice = product.LastPrice[0];
                            }else{
                                productFromContext.LastPrice.Add(product.LastPrice[0]);
                            }
                            if(currentCost>productFromContext.Max){
                                productFromContext.Max = currentCost;
                            }else if(currentCost< productFromContext.Min){
                                productFromContext.Min = currentCost;
                            }
                            _context.Products.Update(productFromContext);
                        }
                        
                            _context.SaveChanges();
                        }
                        
                    }
                    catch (Exception ex)
                    { 
                    }
                }
            }
            catch (Exception ex) when (ex.Message == "Подключение не установлено, т.к.конечный компьютер отверг запрос на подключение Подключение не установлено, т.к.конечный компьютер отверг запрос на подключение")
            {

                if (proxyList.Count-1> index)
                {
                    proxyList.Remove(proxyList[index]);
                }
                ParseProductsFromPage(url,index);
            }
            catch (Exception ex)
            {

                if (index < proxyList.Count - 1)
                {
                    index++;
                    ParseProductsFromPage(url,index++);
                }
            }
            ProxyServerConnectionManagment.SerializeByPuts(proxyList);
        }

        public void GenerateHrefs()
        {
            if (!File.Exists("RozetkaHrefs.txt")) {
                RozetkaMenuDriver RMD = new RozetkaMenuDriver();
                RMD.GetPagesAuto();
            }
        }
    }
}
