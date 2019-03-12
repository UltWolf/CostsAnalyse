using CostsAnalyse.Services;
using System;
using Xunit;

namespace TestsForCostsAnalyse
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(1,ParsingServicesManager.GetListServices().Count);

        }
    }
}
