using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sterling.EdocIntegration.Domain.Common;

namespace PaymentPurchaseManagementService.Service.BackGoundService
{
    public class JobService
    {
        private readonly List<BackGroundJob> _jobs = new();
        private readonly ILogger<JobService> _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;

        public JobService(IConfiguration config, IServiceProvider serviceProvider)
        {
            _config = config;
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ILogger<JobService>>();
        }

        public void AddJob(BackGroundJob job)
        {
            _jobs.Add(job);
            _logger.LogInformation($"Job added: {job.jobId}");
        }

        public IEnumerable<BackGroundJob> GetPendingJobs()
        {
            return _jobs.Where(job => job.status == "Pending" || job.status == "Failed");
        }

        public async Task ProcessJob(BackGroundJob job)
        {
            try
            {
                // Simulate job processing
                _logger.LogInformation($"Processing job {job.jobId}");
                await Task.Delay(500); // Simulated delay

                // Example: Mark job as success or failed
                job.status = "Success";
                job.lastAttemptedAt = DateTime.UtcNow;
                _logger.LogInformation($"Job {job.jobId} processed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing job {job.jobId}: {ex.Message}");
                job.status = "Failed";
                job.lastAttemptedAt = DateTime.UtcNow;
                job.retryCount++;
            }
        }
    }


}
