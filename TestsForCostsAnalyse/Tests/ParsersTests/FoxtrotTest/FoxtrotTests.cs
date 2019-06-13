using CostsAnalyse.Services.PageDrivers;
using CostsAnalyse.Services.Parses;
using CostsAnalyse.Services.ProxyServer;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ParsersTests.FoxtrotTest
{
    public class FoxtrotTests
    {
        [Fact]
        public void ProductsAreNotNull()
        {
            FoxtrotParser rpd = new FoxtrotParser();
            List<String> proxys = ProxyServerConnectionManagment.GetProxyHrefs();
            Assert.NotNull(rpd.GetProduct("https://www.foxtrot.com.ua/ru/shop/mobilnye_telefony_samsung_sm-j320h-galaxy-j3-duos-zdd-gold.html?sc_content=13870_2",0,ref proxys));

        }

        [Fact]
        public void ListProductsAreNotNull() {
           FoxtrotPageDriver FPD = new FoxtrotPageDriver();

            FPD.ParseProductsFromPage("https://www.foxtrot.com.ua/ru/shop/mobilnye_telefony.html", 0);

        }

    }
}
