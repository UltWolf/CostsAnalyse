using CostsAnalyse.Services.PageDrivers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ParsersTests.FoxtrotTest
{
   public  class FoxtrotPageDriverTests
    {
        [Fact]
        public void DontThrowAnyException() {
            FoxtrotPageDriver FPD = new FoxtrotPageDriver(new CostsAnalyse.Models.Context.ApplicationContext());
            FPD.GetPages();
        }
    }
}
