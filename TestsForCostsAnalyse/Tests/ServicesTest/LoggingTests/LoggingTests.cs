using CostsAnalyse.Services.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ServicesTest.LoggingTests
{
    public  class LoggingTests
    {

        FileLogging lg = new FileLogging();
        [Fact]
        public async void IsWriteToFile()
        {
            await lg.LogAsync(new ArgumentException(),new object()); 
            await lg.LogAsync(new ExecutionEngineException(), new object()); 
            await lg.LogAsync(new FieldAccessException(), new object()); 
            await lg.LogAsync(new FormatException(), new object()); 
            await lg.LogAsync(new ArgumentException(), new object());

        }
        [Fact]
        public async void IsReadFile()
        {
            var result = await lg.ReadAsync();
        }
        [Fact]
        public async void IsReadFileByDate()
        {
           var result =  await lg.ReadAsync(DateTime.Now.AddDays(-1));
        }
    }
}

