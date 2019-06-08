using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using CostsAnalyse.Models;

namespace CostsAnalyse.Services.Parses
{
    public class AlloParser : IParser
    {
        public Product GetProduct(string url,int index, ref List<string> proxyList)
        {
            Product product = new Product();
            WebRequest WR = WebRequest.Create(url+ "?tab=specs");
            WR.Method = "GET";
            WR.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:65.0) Gecko/20100101 Firefox/65.0");
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
            var DomDocument = parser.ParseDocument(html);
            var PriceDivs = DomDocument.GetElementsByClassName("price");
            Price price;
            if (PriceDivs.Length > 1)
            {
                var currentPrice = PriceDivs[0].TextContent;
                var previousPrice = PriceDivs[1].TextContent;
                string[] numbers1 = Regex.Split(previousPrice, @"\D+");
                StringBuilder previousPriceString = new StringBuilder();
                for (var i = 0; i < numbers1.Length; i++)
                {
                    previousPriceString.Append(numbers1[i]);
                }
                decimal oldPrice = decimal.Parse(previousPriceString.ToString()); 
                string[] numbers2 = Regex.Split(currentPrice, @"\D+");
                StringBuilder currentPriceString = new StringBuilder();
                for (var i = 0; i < numbers2.Length; i++)
                {
                    currentPriceString.Append(numbers2[i]);
                }
                decimal newPrice = decimal.Parse(currentPriceString.ToString());
                price = new Price(oldPrice, newPrice);
            }
            else
            {
                var currentPrice = PriceDivs[0].TextContent;
                string[] numbers1 = Regex.Split(currentPrice, @"\D+");
                StringBuilder previousPriceString = new StringBuilder();
                for (var i = 0; i < numbers1.Length; i++)
                {
                    previousPriceString.Append(numbers1[i]);
                }
                decimal oldPrice = decimal.Parse(previousPriceString.ToString());
                price = new Price(oldPrice){Company = new Company("Allo", url)};
            }
            product.Name = DomDocument.GetElementById("product-title-h1").TextContent.Replace("Характеристики","");
            var informationBlock = DomDocument.GetElementsByClassName("spec-block")[0];
            var trs = informationBlock.GetElementsByTagName("tr");
            foreach(var tr in trs)
            {
                var title = tr.GetElementsByTagName("td")[0].TextContent;
                var value = tr.GetElementsByTagName("td")[1].TextContent;
                product.AddInformation(title, new Value { Notice = value });
            } 
            product.Price = new List<Price>() { price };
            return product;
        }
    }
}
