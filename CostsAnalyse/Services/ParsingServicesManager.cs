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
        public static List<IParser> GetListServices()
        {
            List<IParser> parsers = new List<IParser>();
            string nspace = "CostsAnalyse.Services.Parses";
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == nspace
                    select t;
            q.ToList().ForEach(m =>
            {
                if (m.IsClass) {
                    Type NameOfType = Type.GetType(m.Name);
                    var InstanceOfParser = Activator.CreateInstance(NameOfType) as IParser;
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
