using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Extensions
{
    public static  class StringExtension
    {
        public static string RemoveWebGarbage(this string line)
        {
            return line.Replace("\t", "").Replace("\n", "").Replace(" ", "");
        }
    }
}
