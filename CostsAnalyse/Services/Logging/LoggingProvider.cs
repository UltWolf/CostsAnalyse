using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Logging
{
    public class LoggingProvider
    {
        public static void InitiateFolder()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
        }
    }
}
