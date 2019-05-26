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
    public class ComfyParser : IParser
    {
        public Product GetProduct(string url)
        { 
            Product product = new Product();
            WebRequest WR = WebRequest.Create(url);
            WR.Method = "GET";
            WR.Headers.Add("Cookie", "visid_incap_1858972=CgSTAAPDSMGXax96dcte+Xy/j1wAAAAAQkIPAAAAAACAYPKKAbBDGObVssVjBap/qaTcXtTWOYIC; fp=11; lfp=3/14/2019, 1:25:18 PM; pa=1552743521644.49220.3629407112534495comfy.ua0.6633585791310551+7; __utma=162514478.1030346563.1552924543.1552983164.1552993964.3; __utmz=162514478.1552983164.2.2.utmcsr=newsletter_12_2019|utmccn=week_12_2019|utmcmd=email|utmcct=259728211; active_city_id=506; incap_ses_247_1858972=rDUWafMCxG4pJn9M1oVtA5/OkFwAAAAAbXhlR3ahAgQVQvZKM95R/g==; X-Store=1; __cfduid=dd441600f6464e9fe6b774a35ae1f0ebb1552895646; userMailID=259728211; __utmc=162514478; __utmb=162514478.1.10.1552993964; __utmt=1");
            WR.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:65.0) Gecko/20100101 Firefox/65.0");
            WebResponse response = WR.GetResponse();
            string  html;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                    
                }
            }
            HtmlParser parser = new HtmlParser();
            var DomDocument = parser.ParseDocument(html);
            product.Name = DomDocument.GetElementsByClassName("product-card__name")[0].TextContent;
             
            var oldPriceDiv = DomDocument.GetElementsByClassName("price-box__content_old");
            Price price;
            if (oldPriceDiv != null)
            {
                var specialPriceDiv = DomDocument.GetElementsByClassName("price-box__content_special");
                var oldPriceElement = oldPriceDiv[0].GetElementsByClassName("price-value")[0].TextContent.Replace("\n","").Replace(" ", "");
                var specialPriceElement = specialPriceDiv[0].GetElementsByClassName("price-value")[0].TextContent.Replace("\n", "").Replace(" ","");
                string[] numbers1 = Regex.Split(oldPriceElement, @"\D+");
                StringBuilder oldPriceString = new StringBuilder(); 
                for(var i = 0; i < numbers1.Length;i++)
                {
                    oldPriceString.Append(numbers1[i]);
                }
                decimal oldPrice = decimal.Parse(oldPriceString.ToString());
                StringBuilder currentPriceString = new StringBuilder();
                string[] numbers2 = Regex.Split(oldPriceElement, @"\D+");
                for (var i = 0; i < numbers2.Length; i++)
                {
                    currentPriceString.Append(numbers2[i]);
                }
                decimal currentPrice = decimal.Parse(currentPriceString.ToString());
                
                

                price = new Price(oldPrice, currentPrice);
            }
            else
            {
               var priceDiv =   DomDocument.GetElementsByClassName("js-item-price");
               string priceElement =  priceDiv[0].GetElementsByClassName("price-value")[0].TextContent;
                string[] numbers1 = Regex.Split(priceElement, @"\D+");
                StringBuilder priceString = new StringBuilder();
                for (var i = 0; i < numbers1.Length; i++)
                {
                    priceString.Append(numbers1[i]);
                }
                decimal currentPrice = decimal.Parse(priceString.ToString());
                price = new Price(currentPrice);
            }
            var TableElement = DomDocument.GetElementById("featuresList");
            var ElementOfTable = DomDocument.GetElementsByClassName("features-item__list-wr");
            product.Price = new List<Price>() { price };
            foreach(var item in ElementOfTable)
            {
                var title = item.GetElementsByClassName("title")[0].TextContent;
                var value = item.GetElementsByClassName("value__item")[0].TextContent;
                product.AddInformation(title, new Value { Notice = value });
            }
            product.Company = new Company("Comfy", url);
            return product;
        }
    }
}
