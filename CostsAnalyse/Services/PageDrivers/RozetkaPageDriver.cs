using AngleSharp.Html.Parser;
using CostsAnalyse.Models;
using CostsAnalyse.Services.Parses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.PageDrivers
{
    public class RozetkaPageDriver
    {
        public List<Product> ParseProductsFromPage(string url)
        {
            var products = new List<Product>();
            WebRequest WR = WebRequest.Create(url);
            WR.Method = "GET";
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
                    return products;
                }

            }
            return products;

        }
    }
}
