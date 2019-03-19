using CostsAnalyse.Services.Parses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ParsersTests.AlloTests
{
    public class AlloParserTests
    {
        [Fact]
        public void ProductAreNotNull()
        {
            AlloParser cp = new AlloParser();
            Assert.NotNull(cp.GetProduct("https://allo.ua/ua/products/mobile/samsung-galaxy-a7-2018-pink-sm-a750fziusek.html"));

        }
    }
}
