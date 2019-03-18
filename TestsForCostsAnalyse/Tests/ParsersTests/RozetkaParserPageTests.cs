using CostsAnalyse.Services.PageDrivers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ParsersTests
{
    public class RozetkaParserPageTests
    {
        [Fact]
        public void ListProductsAreNotNull()
        {
            RozetkaPageDriver rpd = new RozetkaPageDriver();
            Assert.NotNull(rpd.ParseProductsFromPage("https://rozetka.com.ua/notebooks/c80004/filter/"));

        }
    }
}
