using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using CostsAnalyse.Models;
using CostsAnalyse.Models.Data;
using CostsAnalyse.Services.Parses;
using CostsAnalyse.Services.ProxyServer;

namespace CostsAnalyse.Services.Managers
{
    public class ParseManager
    {
        private StateManager _stateManager = new StateManager();
        private List<string> Proxys = new List<string>();

        public ParseManager()
        {
            Proxys = new ProxyBuilder().GenerateProxy().Build();
        }

        public Product Parse(ParseState ps,string url)
        {
            _stateManager.SaveState(ps);
            switch (ps.Type)
            {
                case Store.Comfy:
                    return new ComfyParser().GetProduct(url, ref Proxys);
                    break;
                case Store.Eldorado:
                    throw new NotImplementedException();
                    break;
                case Store.Foxtrot:
                    return new FoxtrotParser().GetProduct(url, ref Proxys);
                    break;
                case Store.Rozetka:
                    return new RozetkaParser().GetProduct(url, ref Proxys);
                    break; 
                default:
                    throw new NotImplementedException();
            }
        }
    }
}