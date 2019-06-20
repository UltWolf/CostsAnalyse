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
        [Theory]
        [InlineData("/ru/shop/mobilnye_telefony_huawei_y6-2019-dual-sim-midnight-black.html")]
        [InlineData("/ru/shop/mobilnye_telefony_xiaomi_redmi-note-7-3-32gb-space-black.html")]
        [InlineData("/ru/shop/mobilnye_telefony_huawei_p30-pro-6-128gb-breathing-srystal.html")]
        public void ProductsAreNotNull(string url)
        {
            FoxtrotParser rpd = new FoxtrotParser();
            List<String> proxys = ProxyServerConnectionManagment.GetProxyHrefs();
            Assert.NotNull(rpd.GetProduct(url,ref proxys));

        }

        [Fact]
        public void ListProductsAreNotNull() {
           FoxtrotPageDriver FPD = new FoxtrotPageDriver(new CostsAnalyse.Models.Context.ApplicationContext());

            FPD.ParseProductsFromPage("https://www.foxtrot.com.ua/ru/shop/mobilnye_telefony.html", 0);

        }

    }
}
