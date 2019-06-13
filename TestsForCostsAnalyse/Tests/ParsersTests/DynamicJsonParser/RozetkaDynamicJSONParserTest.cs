using CostsAnalyse.Services.Parses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ParsersTests.DynamicJsonParser
{
    public class RozetkaDynamicJSONParserTest
    {   [Fact]
        public void ProductAreNotNull() {
            string ApiUrl = "https://rozetka.com.ua/recent_recommends/action=getGoodsDetailsJSON/?goods_ids=p70599548";

            WebRequest WR = WebRequest.Create(ApiUrl);
            WebResponse response = WR.GetResponse(); 
            string json = "";
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                    
                }
            }
            Assert.NotNull(RozetkaDynamicJSONParser.GetProductFromJson(json,""));
        } 
    }
}
