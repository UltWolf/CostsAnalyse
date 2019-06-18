using CostsAnalyse.Services.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ServicesTest.LoggingTests
{
    public class LoggingProviderTests
    { 
        [Fact]
        public void IsCreate()
        {
            LoggingProvider.InitiateFolder();
        } 
    }
}
