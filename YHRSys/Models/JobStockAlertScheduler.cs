using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class JobStockAlertScheduler
    {
        public static void Start() {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail jobStock = JobBuilder.Create<StockAlertJob>()
                .WithIdentity("job2", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                //.WithSchedule
                //(
                //    //Runs 0 59 23 ? * mon == 0:second 59:minutes 23:hours ?:day-of-month *:month mon:day-of-week                    
                //    CronScheduleBuilder.CronSchedule("5 0 0 ? * WED")
                //)
                .WithIdentity("trigger2", "group1")
                .WithPriority(1)
                //.StartAt(DateBuilder.FutureDate(7, IntervalUnit.Day))
                //.WithSimpleSchedule(s => s.WithIntervalInHours(168).RepeatForever())
                //.WithCronSchedule("0 59 23 ? * FRI")
                //.WithCronSchedule("5 0/1 * * * ?")

                .WithCronSchedule("0 59 23 ? * FRI", x => x
                    .WithMisfireHandlingInstructionFireAndProceed())
                //.WithCronSchedule("0 09 11 ? * WED", x => x
                //    .WithMisfireHandlingInstructionFireAndProceed())
                //.ForJob("job2", "group1")
                .Build();

            Debug.WriteLine("StockAlert-Scheduler");
            scheduler.ScheduleJob(jobStock, trigger);
        }
    }
}