using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace UserAuthManagementService.Service.BackGoundService
{
    public class JobWorker : BackgroundService
    {
        private readonly JobService _jobService;

        public JobWorker(JobService jobService)
        {
            _jobService = jobService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var jobs = _jobService.GetPendingJobs();

                foreach (var job in jobs)
                {
                    if (job.status == "Pending" && (DateTime.UtcNow - job.createdAt).TotalMinutes >= 5)
                    {
                        await _jobService.ProcessJob(job);
                    }
                    else if (job.status == "Failed" && job.lastAttemptedAt.HasValue &&
                             (DateTime.UtcNow - job.lastAttemptedAt.Value).TotalMinutes >= 15 && job.retryCount < 4)
                    {
                        await _jobService.ProcessJob(job);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Check every minute
            }
        }
    }
}
