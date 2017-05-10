using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;

namespace YHRSys.Models
{
    public class ActivityLogClosureScheduler : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            CheckGroupAffiliate.CloseActivityLog();
        }
    }
}