using CostsAnalyse.Services.Parses;
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
            Assert.NotNull(rp.GetProduct("https://rozetka.com.ua/lenovo_tab4_za310144ua/p31091351/"));

        }
    }
}
