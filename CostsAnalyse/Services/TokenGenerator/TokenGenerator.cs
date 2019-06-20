using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.TokenBuilder
{
    public static  class TokenGenerator
    {
        public static string Generate()
        {
            Random random = new Random();
            int length = 12;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
