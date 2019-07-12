using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Abstracts
{
    public interface ITask
    {
        Task RunAtTimeOf(DateTime now);
    }
}
