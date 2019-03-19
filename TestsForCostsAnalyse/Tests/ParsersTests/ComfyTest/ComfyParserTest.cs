using CostsAnalyse.Services.Parses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ParsersTests.ComfyTest
{
   public  class ComfyParserTest
    {
        [Fact]
        public void ProductAreNotNull()
        {
            ComfyParser cp = new ComfyParser();
            Assert.NotNull(cp.GetProduct("https://comfy.ua/stiral-naja-mashina-lg-fh0b8nd1.html"));

        }
    }
}
