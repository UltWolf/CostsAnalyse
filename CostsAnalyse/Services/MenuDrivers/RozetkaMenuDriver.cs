using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using CostsAnalyse.Models;
using CostsAnalyse.Services.ProxyServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CostsAnalyse.Services
{
    public class RozetkaMenuDriver
    {
        private List<string> proxys;
        private static RozetkaMenuDriver _instanse;
        private static  object _locker = new object();
        private RozetkaMenuDriver()
        {
            proxys = new ProxyBuilder().GenerateProxy().Build();
        }

        public static RozetkaMenuDriver GetInstanse()
        {
            lock (_locker)
            {
                if (_instanse == null)
                {
                    lock (_locker)
                    {
                        _instanse = new RozetkaMenuDriver();
                    }
                }
            }

            return _instanse;
        }
        public void getPages()
        {
            HashSet<string> pages = new HashSet<string>();
            pages.Add("https://rozetka.com.ua/ua/notebooks/c80004/filter/");
            pages.Add("https://rozetka.com.ua/ua/tablets/c130309/filter/");
            pages.Add("https://rozetka.com.ua/ua/notebook-bags/c80036/");
            using (FileStream fs = new FileStream("RozetkaHrefs.txt", FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, pages);
            }
        }

        public void GetPagesAuto()
        {
            
            HashSet<String> listOfHrefs = new HashSet<string>();
            if (proxys.Count > 0)
            {
          
                foreach (var proxy in proxys)
                {
                    try
                    {
                        WebRequest webRequest = WebRequest.Create("https://rozetka.com.ua/ua/all-categories-goods/");
                        webRequest.Method = "GET";
                        WebProxy web = new WebProxy();
                        string[] fulladress = proxy.Split(":");
                        var (adress, port) = (fulladress[0], int.Parse(fulladress[1]));
                        WebProxy prox = new WebProxy(adress, port);
                        prox.BypassProxyOnLocal = false;
                        webRequest.Proxy = prox;


                        string html = "";
                        ThreadDelay.Delay();
                        using (var response = webRequest.GetResponse())
                        {
                            using (var streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                html = streamReader.ReadToEnd();
                            }
                        }
                        HtmlParser parser = new HtmlParser();
                        var htmlDocument = parser.ParseDocument(html);
                        var bodyDiv = htmlDocument.GetElementsByClassName("all-cat-b-l-i-parent");
                 
                        if (bodyDiv != null)
                        {
                            GetInsidingHrefsInHrefs(bodyDiv, ref listOfHrefs);
                        }
                        break;
                    }
                    catch(Exception ex) { }
                }
               

            }
        }
        public List<string> GetPagesFromFile()
        {
            List<string> hrefs = new List<string>();
            using (FileStream fs = new FileStream("RozetkaHrefs.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                hrefs = (List<String>)bf.Deserialize(fs);

            }
            return hrefs;
        }

        private void GetInsidingHrefsInHrefs(IHtmlCollection<IElement> hrefs, ref HashSet<string> listOfHrefs)
        {
            HtmlParser parser = new HtmlParser(); 
            if (proxys.Count > 0)
            {

                foreach (var href in hrefs)
                { 
                    for (int i=0; i<proxys.Count-1;i++)
                    {
                        try
                        {
                            ThreadDelay.Delay();
                            string fullHref = href.GetElementsByTagName("a")[0].GetAttribute("href");
                            var webRequest = WebRequest.Create(fullHref);
                            string[] fulladress = proxys[i].Split(":");
                            var (adress, port) = (fulladress[0], int.Parse(fulladress[1]));
                            WebProxy prox = new WebProxy(adress, port);
                            prox.BypassProxyOnLocal = false;
                            webRequest.Proxy = prox;
                            using (var response = webRequest.GetResponse())
                            {

                                string html = "";
                                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                                {
                                    html = streamReader.ReadToEnd();
                                    var htmlDocument = parser.ParseDocument(html);

                                    if (htmlDocument.GetElementById("block_with_goods") != null)
                                    {
                                        listOfHrefs.Add(fullHref);
//analog break
                                        i = proxys.Count;
                                    }
                                    else
                                    {
                                        var portal = htmlDocument.GetElementsByClassName("portal-automatic");
                                        if (portal.Length != 0)
                                        {
                                            var p_auto_block = portal[0].GetElementsByClassName("p-auto-block");
                                            foreach (var p_auto in p_auto_block)
                                            {
                                               foreach( var p in p_auto.GetElementsByClassName("arrow-link"))
                                                {
                                                    listOfHrefs.Add(p.GetAttribute("href"));
                                                }
                                            }
                                            //if (p_auto_block.Length != 0)
                                            //{
                                            //    foreach(var p in p_auto_block)
                                            //    {
                                            //        var table = p.GetElementsByClassName("pab-table");
                                            //        if (table.Length != 0)
                                            //        {
                                            //            foreach (var tab in table)
                                            //            {
                                            //                var imghref = tab.GetElementsByClassName("pab-img");
                                            //                if (imghref.Length != 0) {
                                            //                    foreach (var img in imghref)
                                            //                    {

                                            //                        var hrefFromImg = img.GetElementsByTagName("a");
                                            //                        if (hrefFromImg.Length != 0) {
                                            //                            var hrefToProduct = hrefFromImg[0].GetAttribute("href");
                                            //                            if(hrefToProduct.)
                                            //                            listOfHrefs.Add();
                                            //                        }

                                            //                    }
                                            //  }
                                            //}

                                            //}
                                            //}
                                            //}
                                            i = proxys.Count;
                                        }
                                     
                                    }
                                }
                            }
                        }
                        catch (Exception ex) { }

                    }

                }
                }
                using (FileStream fs = new FileStream("RozetkaHrefs.txt", FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, listOfHrefs.ToList());
                }
            
        }
    }
}

