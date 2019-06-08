using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CostsAnalyse.Services
{
    public static class ThreadDelay
    {
        public static void Delay() {
            Thread.Sleep(1000 * new Random().Next(7));
        }
    }
}
