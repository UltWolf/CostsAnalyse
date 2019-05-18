﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using CostsAnalyse.Models;

namespace CostsAnalyse.Services.Parses
{
    public class FoxtrotParser : IParser
    {
        public Product GetProduct(string url)
        {
            Product product = new Product();
            WebRequest WR = WebRequest.Create(url);
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
            var discont = DomDocument.GetElementsByClassName("price__not-relevant");
            Price price;
            if (discont != null)
            {
                decimal oldPrice = decimal.Parse(discont[0].GetElementsByClassName("numb")[0].TextContent);
                var  currentDiv = DomDocument.GetElementsByClassName("price__relevant");
                decimal currentPrice = decimal.Parse(currentDiv[0].GetElementsByClassName("numb")[0].TextContent);
                price = new Price(oldPrice, currentPrice);
            }
            else
            {
                var currentDiv = DomDocument.GetElementsByClassName("price__relevant");
                decimal currentPrice = decimal.Parse(currentDiv[0].GetElementsByClassName("numb")[0].TextContent);
                price = new Price(currentPrice);
            }
            product.Price = price;
            var divCharacter = DomDocument.GetElementsByClassName("characteristic__block")[0];
            var lis = divCharacter.GetElementsByTagName("li");
            foreach(var li in lis)
            {
                var title = li.GetElementsByTagName("p")[0].TextContent;
                var value = li.GetElementsByTagName("p")[1].TextContent;
                product.AddInformation(title, new Value { Notice = value }); 
            }
            product.Company = new Company("Foxtrot", url);
            return product;

        }
    }
}