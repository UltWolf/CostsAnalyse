using CostsAnalyse.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.Logging
{
    public class FileLogging : ILogging
    {
        private const int DefaultBufferSize = 4096;
        public async Task LogAsync(Exception ex, object obj)
        {
            string pathToFile = "Logs/" + DateTime.Today.ToString("d").Replace("/", "") + ".log";
            string message = " ";
            message += DateTime.Now + "  -  ";
            message += ex.Message + ";\n";
            message += "State: " + obj.ToString();
            var buffer = Encoding.UTF8.GetBytes(message);
                using (FileStream fs = new FileStream(pathToFile, FileMode.Append,FileAccess.Write,FileShare.None, buffer.Length, true))
                {
                await fs.WriteAsync(buffer);
                } 
        }

        public async Task<String[]> ReadAsync()
        {
            return await ReadAsync(DateTime.Now);
        }

        public async Task<String[]> ReadAsync(DateTime date)
        {
            var lines = new List<string>();
            string path = "Logs/" + DateTime.Today.ToString("d").Replace("/", "") + ".log";
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }
 
    }
}
