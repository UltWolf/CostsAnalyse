using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Abstracts
{
    public interface ILogging
    {
         Task LogAsync(Exception ex, object obj);
         Task<String[]> ReadAsync();
         Task<String[]> ReadAsync(DateTime date);
    }
}
