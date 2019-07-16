using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsAnalyse.Services.ProxyServer;

namespace CostsAnalyse.Services.Initializers
{
    public class StartsInitialize
    {
        public static void Initialize()
        {
            new ProxyBuilder().GenerateProxy().Build();
        }
    }
}
