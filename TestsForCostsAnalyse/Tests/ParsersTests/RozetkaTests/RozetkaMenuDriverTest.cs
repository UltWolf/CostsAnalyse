using CostsAnalyse.Services;
using CostsAnalyse.Services.PageDrivers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ParsersTests
{
    public class RozetkaPageDriverTest
    {
        RozetkaMenuDriver RMD = new RozetkaMenuDriver();
        [Fact]
        public void SetPageWithOutExceptions() { 
            RMD.GetPagesAuto();
        }
        [Fact]
        public void GetWithoutException()
        {
            RMD.GetPagesFromFile();

        }
    }
}
