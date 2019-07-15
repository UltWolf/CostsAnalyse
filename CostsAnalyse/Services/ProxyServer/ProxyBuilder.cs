using System;
using System.Collections.Generic;

namespace CostsAnalyse.Services.ProxyServer
{
    public class ProxyBuilder
    {
        private bool IsGenerate;

        public  ProxyBuilder GenerateProxy()
        {
            ProxyServerConnectionManagment.GenerateProxy();
            IsGenerate = true;
            return this;
        }

        public List<string> Build()
        {
            if (IsGenerate)
                return ProxyServerConnectionManagment.GetUrlsList();
            else
            {
                throw new Exception("Urls didn`t generate");
            }
        }
      
    }
}