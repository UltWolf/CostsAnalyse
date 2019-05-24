using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Initializers.Basic
{
    public interface IInitialize
    {
        void Initialize(IServiceProvider serviceProvider);
    }
}
