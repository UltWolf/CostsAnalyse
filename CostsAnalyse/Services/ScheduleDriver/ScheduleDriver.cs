using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.ScheduleDriver
{
    public class ScheduleDriver
    {
        public static void ScheduleReccuringJob()
        { 
            RecurringJob.AddOrUpdate<TaskDriver>(nameof(TaskDriver), 
                job => job.Run(JobCancellationToken.Null)  ,
                Cron.MinuteInterval(2),TimeZoneInfo.Local);

        }
    }
}
