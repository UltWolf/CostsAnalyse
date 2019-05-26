using CostsAnalyse.Services.PageDrivers;
using CostsAnalyse.Services.Parses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CostsAnalyse.Services
{
    public static class ParsingServicesManager
    {
        public static List<IPageDrivers> GetListServices()
        {
            List<IPageDrivers> parsers = new List<IPageDrivers>();
            string nspace = "CostsAnalyse.Services.PageDrivers";
            var q = from t in Assembly.GetExecutingAssembly().GetTypes() 
            where t.IsClass && t.Namespace == nspace
                    select t;
            q.ToList().ForEach(m =>
            {
                if (m.IsClass)
                {
                    string name = "CostsAnalyse.Services.PageDrivers." + m.Name + ",CostsAnalyse";
                    Type NameOfType = Type.GetType(name);
                    var InstanceOfParser = Activator.CreateInstance(NameOfType) as IPageDrivers;
                    if (InstanceOfParser != null)
                    {
                        parsers.Add(InstanceOfParser);
                    }
                }
            }
            );
            return parsers;
        }
    }
}
