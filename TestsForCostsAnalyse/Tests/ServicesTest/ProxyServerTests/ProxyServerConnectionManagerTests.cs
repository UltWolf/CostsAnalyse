using CostsAnalyse.Services.ProxyServer;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ProxyServerTests
{
    public class ProxyServerConnectionManagerTests
    {
        [Fact]
        public void NoExceptionToWorkWithInitiateProxy()
        {
            new ProxyBuilder().GenerateProxy().Build();
        }
         

    }
}
