using CostsAnalyse.Services.TokenBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ServicesTest.TokenGeneratorTest
{
    public class TokenGeneratorTests
    {
        [Fact]
        public void GenerateEquaLength()
        {
            int expectedLength = 12;
            Assert.Equal(TokenGenerator.Generate().Length,expectedLength);
        }
    }
}
