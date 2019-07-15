using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using CostsAnalyse.Models.Data;
using CostsAnalyse.Services.Parses;

namespace CostsAnalyse.Services.Managers
{
    public class ParseManager
    {
        private StateManager StateManager = new StateManager();

        public void  Parse(string url,int index, Store type)
        {
            switch (type)
            {
                case Store.Comfy:
                    var product = new ComfyParser().GetProduct(url);
                    
                    break;
                 case Store.Eldorado:
                     break;
                 case Store.Foxtrot:
                     break;
                 case Store.Rozetka:
                     break;
                 default:
                     break;
            }
            
        }
    }
}