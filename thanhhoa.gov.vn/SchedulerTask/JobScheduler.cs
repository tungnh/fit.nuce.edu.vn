using Quartz;
using Quartz.Impl;
using System;

namespace thanhhoa.gov.vn.SchedulerTask
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<BackupJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("triggerBackup", "backup")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}