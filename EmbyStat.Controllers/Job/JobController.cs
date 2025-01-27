﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EmbyStat.Common;
using EmbyStat.Common.Hubs.Job;
using EmbyStat.Common.Models.Tasks.Enum;
using EmbyStat.Jobs;
using EmbyStat.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace EmbyStat.Controllers.Job
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class JobController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IJobService _jobService;
        private readonly IJobHubHelper _jobHubHelper;
        private readonly IJobInitializer _jobInitializer;
        private string LogPrefix => Constants.LogPrefix.JobController;

        public JobController(IMapper mapper, IJobService jobService, IJobHubHelper jobHubHelper,
            IJobInitializer jobInitializer)
        {
            _mapper = mapper;
            _jobService = jobService;
            _jobHubHelper = jobHubHelper;
            _jobInitializer = jobInitializer;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            var jobs = _jobService.GetAll();
            return Ok(_mapper.Map<IList<JobViewModel>>(jobs));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            var job = _jobService.GetById(id);
            if (job == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<JobViewModel>(job));
        }

        [HttpPatch]
        [Route("{id}")]
        public IActionResult UpdateTrigger(Guid id, string cron)
        {
            if (_jobService.UpdateTrigger(id, cron))
            {
                _jobInitializer.UpdateTrigger(id, cron);
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost]
        [Route("fire/{id}")]
        public async Task<IActionResult> FireTask(Guid id)
        {
            var job = _jobService.GetById(id);
            if (job == null)
            {
                return NotFound();
            }

            await Task.Run(() => { RecurringJob.Trigger(job.Id.ToString());});
            await _jobHubHelper.BroadCastJobLog(LogPrefix, $"{GetJobTitle(job.Title)} job queued", ProgressLogType.Information);
            return Ok();
        }

        private string GetJobTitle(string key)
        {
            switch (key)
            {
                case "DATABASECLEANUPTITLE": return Constants.LogPrefix.DatabaseCleanupJob;
                case "CHECKUPDATETITLE": return Constants.LogPrefix.CheckUpdateJob;
                case "SMALLEMBYSYNCTITLE": return Constants.LogPrefix.SmallEmbySyncJob;
                case "MEDIASYNCTITLE": return Constants.LogPrefix.MediaSyncJob;
                case "PINGEMBYSERVERTITLE": return Constants.LogPrefix.PingEmbyJob;
                default: break;
            }

            return string.Empty;
        }

        [HttpGet]
        [Route("mediasync")]
        public IActionResult IsFireMediaSyncRunning()
        {
            var job = _jobService.GetById(Constants.JobIds.MediaSyncId);
            return Ok(_mapper.Map<JobViewModel>(job));
        }
    }
}