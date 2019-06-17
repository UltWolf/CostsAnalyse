using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.PageDrivers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ParsersTests.RozetkaTests
{
    public class RozetkaPageDriverTests
    {
        [Fact]
        public void GetListWithoutExceptions()
        {
            RozetkaPageDriver RPD = new RozetkaPageDriver(new ApplicationContext());
            RPD.ParseProductsFromPage("https://rozetka.com.ua/notebooks/c80004/filter", 0);
        }
    }
}
