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
using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using CostsAnalyse.Services.ProxyServer;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Models.Data;
using CostsAnalyse.Services.Logging;
using CostsAnalyse.Services.Managers;

namespace CostsAnalyse.Services.PageDrivers
{
    public class RozetkaPageDriver : IPageDrivers
    {
        List<string> proxyList = new List<string>();
        private readonly ApplicationContext _context;
        private readonly Repositories.ProductRepository _productRepository;
        public RozetkaPageDriver(ApplicationContext Context)
        {
            this.proxyList = new ProxyBuilder().GenerateProxy().Build();
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
            var _stateManager = new StateManager();
            for(int i=0;i<pages.Count;i++)
            {
                _stateManager.SaveState(new ParseState(i,Store.Rozetka));
                ParseProductsFromPage(pages.GetItemByIndex(i), i);
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
                IHtmlDocument document;
                var divsWithProduct = GetPageWithProduct(url,out document); 
                GetProductsFromPage(divsWithProduct);
                var paginator = document.GetElementsByClassName("paginator-catalog");
                string page = GetNumberPage(paginator);
                
                if (page != "")
                {
                    ParseProductsFromPage(baseUrl, int.Parse(page));
                }
            } 
            catch (Exception ex)
            {
               Logging.FileLogging fl = new FileLogging();
               fl.LogAsync(ex, new {url, index});
            } 
        }           
        
        private IHtmlCollection<IElement> GetPageWithProduct(string url,out IHtmlDocument htmlDocument)
        {
            
            foreach (var proxy in proxyList)
            {
                try
                {
                    WebRequest WR = WebRequest.Create(url);
                    WR.Method = "GET";
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
                    htmlDocument = parser.ParseDocument(html);
                    return   htmlDocument.GetElementsByClassName("g-i-tile-catalog");
                }
                catch (Exception ex)
                { 
                }
            }

            throw new Exception("Proxy isn`t working");
        }

        private void GetProductsFromPage(IHtmlCollection<IElement> divsWithProduct)
        {
           
            foreach (var div in divsWithProduct)
            { 
                GetProductFromDiv(div);
            }
        }

        private void GetProductFromDiv(IElement div)
        {
            try
            {   ParseManager parseManager = new ParseManager();
                string urlForPage = div.GetElementsByClassName("g-i-tile-i-title")[0].GetElementsByTagName("a")[0].GetAttribute("href");
                var product = parseManager.Parse( Store.Rozetka,urlForPage);
                _productRepository.AddProduct(product); 
            }
            catch (Exception ex)
            {
            } 
        }

        private string GetNumberPage(IHtmlCollection<IElement> paginator)
        {
            if (paginator.Length > 0)
            {
                var lis = paginator[0].GetElementsByTagName("li");
                for (int i = 0; i < lis.Length; i++)
                {
                    if (lis[i].ClassList.Contains("active"))
                    {
                        return lis[++i].GetElementsByTagName("span")[0].TextContent; 
                    }
                }
            }

            return "";
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
