
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsAnalyse.Models;

namespace CostsAnalyse.Services.Parses
{
    public class RozetkaDynamicJSONParser
    {
        public static Product GetProductFromJson(string jsonContent,string url)
        {
            Product product = new Product();
            var jObject = JObject.Parse(jsonContent);
            var content = (JObject)((JArray)jObject.Property("content")
                                            .Value)[0];
            var contentInsideContent = (JObject)content.Last.Last;
            product.Name = contentInsideContent.Property("title_only").Value.ToString();
            product.Index = product.Name.Split("(")[1].Split(")")[0];
            int oldPrice = int.Parse(contentInsideContent.Property("old_price").Value.ToString()) ;
            decimal price = decimal.Parse(contentInsideContent.Property("price").Value.ToString());

            if (oldPrice > 0)
            {
                product.LastPrice = new List<Price>() { new Price(
                    price,
                    oldPrice,
                    new Company("Rozetka", url))
                    { IsDiscont=true,
                     Discont =  Math.Truncate((1-(price/oldPrice))*100)} };
            }
            else
            {
                product.LastPrice = new List<Price>() { new Price(
                    price, 
                    new Company("Rozetka", url))
                    { IsDiscont=false} };
            }
          
            product.UrlImage = ((JObject)contentInsideContent.Property("large_image").Value).Property("url").Value.ToString();
            product.Category =  contentInsideContent.Property("parent_title").Value.ToString();


            return new Product(); ;
        }
    }
}
