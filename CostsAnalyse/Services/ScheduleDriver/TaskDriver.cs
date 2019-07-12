using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.Abstracts;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.ScheduleDriver
{
    public class TaskDriver : ITask
    {
        private readonly ILogger<ITask> _logger;
        private readonly ApplicationContext _context;

        public TaskDriver(  ApplicationContext context)
        { 
            _context = context;
        }
        public async Task RunAtTimeOf(DateTime now)
        {
            //_logger.LogInformation("Task from schedule start.");

            //_logger.LogInformation("Task have been finished.");
        }
        public async  Task Run(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await RunAtTimeOf(DateTime.Now);
        }
    }
}
