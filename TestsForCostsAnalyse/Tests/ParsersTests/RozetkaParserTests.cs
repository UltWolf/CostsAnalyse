using CostsAnalyse.Services.Parses;
using CostsAnalyse.Services.ProxyServer;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ParsersTests
{
    public class RozetkaParserTests
    {
        [Fact]
        public void ProductAreNotNull()
        {   RozetkaParser rp = new RozetkaParser();

            Assert.NotNull(rp.GetProductWithoutProxy("https://rozetka.com.ua/lenovo_tab4_za310144ua/p31091351/"));

        }
        [Fact]
        public void ProductWithoutDiscontAreNotNull()
        {
            Assert.NotNull(new RozetkaParser().GetProductWithoutProxy("https://rozetka.com.ua/ua/asus_d540na_gq211t/p70599548/"));
        }
    }
}
