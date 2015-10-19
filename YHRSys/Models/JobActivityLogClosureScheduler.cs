using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;


namespace YHRSys.Models
{
    public class JobActivityLogClosureScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail jobActivityLog = JobBuilder.Create<ActivityLogClosureScheduler>()
                .WithIdentity("job1", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                //.WithSchedule
                //(
                ////Runs 0 59 23 ? * mon == 0:second 59:minutes 23:hours ?:day-of-month *:month monday:day-of-week                    
                //    CronScheduleBuilder.CronSchedule("8 0 0 ? * WED")
                //)
                .WithIdentity("trigger1", "group1")
                .WithPriority(1)
                //.StartAt(DateBuilder.FutureDate(7, IntervalUnit.Day))
                //.WithSimpleSchedule(s => s.WithIntervalInHours(168).RepeatForever())
                //.WithCronSchedule("0 59 23 ? * MON")
                //.WithCronSchedule("5 0/1 * * * ?")
                .WithCronSchedule("0 59 23 ? * MON", x => x
                    .WithMisfireHandlingInstructionFireAndProceed())
                .WithCronSchedule("0 30 7 ? * TUE", x => x
                    .WithMisfireHandlingInstructionFireAndProceed())
                //.ForJob("job2", "group1")
                .Build();

            Debug.WriteLine("ActivityClosure-Scheduler");
            scheduler.ScheduleJob(jobActivityLog, trigger);
        }
    }
}