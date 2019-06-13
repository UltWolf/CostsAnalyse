using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using AngleSharp.Html.Parser;
using CostsAnalyse.Models;
using CostsAnalyse.Services.ProxyServer;
using Newtonsoft.Json;

namespace CostsAnalyse.Services.Parses
{
    public class RozetkaParser : IParser
    {
        public RozetkaParser()
        {
        }
        public Product GetProduct(string url, ref List<string> proxyList)
        {
            ThreadDelay.Delay();
            var splitedUrl = url.Split('/');
            var ProductId = splitedUrl[splitedUrl.Length - 2].Remove(0, 1);
            string ApiUrl = "https://rozetka.com.ua/recent_recommends/action=getGoodsDetailsJSON/?goods_ids=" + ProductId;
            Product product = new Product();
            foreach (var proxy in proxyList)
            {
                try
                {
                    WebRequest WR = WebRequest.Create(ApiUrl);
                    WR.Method = "GET";
                    SetProxy(ref WR,  proxy);
                    product = GetProductInstance(WR, url);
                    return product;
                }
                catch (Exception ex)
                {

                }
            }
            ProxyServerConnectionManagment.SerializeByPuts(proxyList);
            return product;
        }
        
        public Product GetProductWithoutProxy(string url)
        {
            ThreadDelay.Delay();
            var splitedUrl = url.Split('/');
            var ProductId = splitedUrl[splitedUrl.Length - 2].Remove(0, 1);
            string ApiUrl = "https://rozetka.com.ua/recent_recommends/action=getGoodsDetailsJSON/?goods_ids=" + ProductId;

            WebRequest WR = WebRequest.Create(ApiUrl);
            WR.Method = "GET";
            Product product = GetProductInstance(WR,url);

            return product;
        }
        public void SetProxy(ref WebRequest wr,  string proxy)
        {
            string[] fulladress = proxy.Split(":");
            var (adress, port) = (fulladress[0], int.Parse(fulladress[1]));
            WebProxy prox = new WebProxy(adress, port);
            prox.BypassProxyOnLocal = false;
            wr.Proxy = prox;

        }
        public Product GetProductInstance(WebRequest WR,string url) {

            Product product = new Product();
            WebResponse response = WR.GetResponse();
            string json;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        json = reader.ReadToEnd();
                    }
                    catch (Exception ex) {
                        return null;
                    }
                }
            }
            product = RozetkaDynamicJSONParser.GetProductFromJson(json,url);
            GetCharacteristics(WR, url, ref product);
            return product;
        }
        public void GetCharacteristics(WebRequest WR, string url, ref Product product)
        {
            WR = WebRequest.Create(url + "/#tab=characteristics");
            WR.Method = "GET";
            WebResponse response = WR.GetResponse();
            string HtmlDom = "";
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    HtmlDom = reader.ReadToEnd();
                }
            }
            response.Close();
            var parser = new HtmlParser();
            var parseDom = parser.ParseDocument(HtmlDom);
            var table = parseDom.GetElementsByTagName("table");
            if (table.Length != 0)
            {
                var trs = table[0].GetElementsByTagName("tr");
                foreach (var tr in trs)
                {
                    var ng = tr.GetElementsByClassName("ng-star-inserted");
                    string title = ng[0].TextContent;
                    string value = "";
                    int i = 1;
                    do
                    {
                        string valueFromResponse = tr.GetElementsByClassName("ng-star-inserted")[i].TextContent;
                        if (value == "")
                        {
                            value = valueFromResponse;
                        }
                        else {
                            if (!value.Contains(valueFromResponse)){
                                value += ";" + valueFromResponse;
                            }
                       }
                        i++;
                        

                    } while (i < ng.Length - 1);
                    if (value != "")
                    {
                        product.AddInformation(title, new Value { Notice = value });
                    }
                }
            }
        }
    }
}
