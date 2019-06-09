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
        public void ListProductsAreNotNull()
        {
            FoxtrotParser rpd = new FoxtrotParser();
            List<String> proxys = ProxyServerConnectionManagment.GetProxyHrefs();
            Assert.NotNull(rpd.GetProduct("https://www.foxtrot.com.ua/ru/shop/mobilnye_telefony_samsung_sm-j320h-galaxy-j3-duos-zdd-gold.html?sc_content=13870_2",0,ref proxys));

        }
    }
}
