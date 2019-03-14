using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using CostsAnalyse.Models;
using Newtonsoft.Json;

namespace CostsAnalyse.Services.Parses
{
    public class RozetkaParser : IParser
    {
        public Product GetProduct(string url)
        {
            var splitedUrl = url.Split('/');
            var ProductId = splitedUrl[splitedUrl.Length - 2].Remove(0, 1);
            string ApiUrl = "https://rozetka.com.ua/recent_recommends/action=getGoodsDetailsJSON/?goods_ids=" + ProductId;
            Product product = new Product();
            WebRequest WR = WebRequest.Create(ApiUrl);
            WR.Method = "GET";
            WebResponse response = WR.GetResponse();
            ParseRozetkaElement html;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    html = JsonConvert.DeserializeObject<ParseRozetkaElement>(json);
                }
            }
            product.Name = html.content[0].content.title_only;
            product.Price = new Price(decimal.Parse(html.content[0].content.price), html.content[0].content.old_price);
            product.Company = new Company("Rozetka", url);

            product.Category = html.content[0].content.parent_title;

            WR = WebRequest.Create(url + "/#tab=characteristics");
            WR.Method = "GET";
            response = WR.GetResponse();
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
            var table = parseDom.GetElementsByClassName("chars-t")[0];
            var trs = table.GetElementsByTagName("tr");
            foreach (var tr in trs)
            {
                string title = tr.GetElementsByClassName("chars-t-cell")[0].TextContent;
                string value = tr.GetElementsByClassName("chars-value-inner")[0].TextContent;
                product.AddInformation(title, value);

            }
            return product;
        }
    }
}
