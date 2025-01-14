﻿using System;
using System.Threading.Tasks;
using EmbyStat.Common;
using EmbyStat.Common.Hubs.Job;
using EmbyStat.Jobs.Jobs.Interfaces;
using EmbyStat.Repositories.Interfaces;
using EmbyStat.Services.Interfaces;
using Hangfire;

namespace EmbyStat.Jobs.Jobs.Maintenance
{
    [DisableConcurrentExecution(5 * 60)]
    public class DatabaseCleanupJob : BaseJob, IDatabaseCleanupJob
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IPersonRepository _personRepository;

        public DatabaseCleanupJob(IJobHubHelper hubHelper, IJobRepository jobRepository, ISettingsService settingsService,
            IStatisticsRepository statisticsRepository, IPersonRepository personRepository) 
            : base(hubHelper, jobRepository, settingsService)
        {
            _statisticsRepository = statisticsRepository;
            _personRepository = personRepository;
            Title = jobRepository.GetById(Id).Title;
        }

        public sealed override Guid Id => Constants.JobIds.DatabaseCleanupId;
        public override string JobPrefix => Constants.LogPrefix.DatabaseCleanupJob;
        public override string Title { get; }

        public override async Task RunJobAsync()
        {
            _statisticsRepository.CleanupStatistics();
            await LogProgress(50);
            await LogInformation("Removed old statistic results.");
        }
    }
}