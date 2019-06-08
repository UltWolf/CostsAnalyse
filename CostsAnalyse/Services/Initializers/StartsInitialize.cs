using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Initializers
{
    public class StartsInitialize
    {
        public static void Initialize()
        {
            ProxyServer.ProxyServerConnectionManagment.SerialiseProxyServers(false);
        }
    }
}
