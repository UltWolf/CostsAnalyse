using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
        public Product GetProduct(string url, int index, ref List<string> proxyList)
        {
            ThreadDelay.Delay();
            var splitedUrl = url.Split('/');
            var ProductId = splitedUrl[splitedUrl.Length - 2].Remove(0, 1);
            string ApiUrl = "https://rozetka.com.ua/recent_recommends/action=getGoodsDetailsJSON/?goods_ids=" + ProductId;
            Product product = new Product();
            try
            {
                WebRequest WR = WebRequest.Create(ApiUrl);
                WR.Method = "GET";
                SetProxy(ref WR, index, ref proxyList);
                product = GetProductInstance(WR, url);
                return product;
            }
            catch (Exception ex) when ((ex.Message == "Подключение не установлено, т.к.конечный компьютер отверг запрос на подключение Подключение не установлено, т.к.конечный компьютер отверг запрос на подключение") || (ex.Message == "The remote server returned an error: (503) Too many open connections."))
            {

                if (proxyList.Count - 1 > index)
                {
                    proxyList.Remove(proxyList[index]);
                }
                return GetProduct(url, index, ref proxyList);
            }
            catch (Exception ex)
            {

                if (index < proxyList.Count - 1)
                {
                    index++;
                    return GetProduct(url, index++, ref proxyList);
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
        public void SetProxy(ref WebRequest wr, int index, ref List<string> proxyList)
        {
            string[] fulladress = proxyList[++index].Split(":");
            var (adress, port) = (fulladress[0], int.Parse(fulladress[1]));
            WebProxy proxy = new WebProxy(adress, port);
            proxy.BypassProxyOnLocal = false;
            wr.Proxy = proxy;

        }
        public Product GetProductInstance(WebRequest WR,string url) {

            Product product = new Product();
            WebResponse response = WR.GetResponse();
            ParseRozetkaElement html;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    try
                    {
                        html = JsonConvert.DeserializeObject<ParseRozetkaElement>(json);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            product.Name = html.content[0].content.title_only;
            product.Index = product.Name.Split("(")[1].Split(")")[0];
            try
            {
                int oldPrice = html.content[0].content.old_price;
                decimal price = decimal.Parse(html.content[0].content.price);

                product.LastPrice = new List<Price>() { new Price(
                    price,
                    oldPrice,
                    new Company("Rozetka", url))
                    { IsDiscont=true,
                     Discont =  Math.Truncate((1-(price/oldPrice))*100)} };

            }
            catch (Exception ex)
            {
                product.LastPrice = new List<Price>() { new Price(decimal.Parse(html.content[0].content.price), new Company("Rozetka", url)) };
            }
            product.UrlImage = html.content[0].content.large_image.url;
            product.Category = html.content[0].content.parent_title;
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
