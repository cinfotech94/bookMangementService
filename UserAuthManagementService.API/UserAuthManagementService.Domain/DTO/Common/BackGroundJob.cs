using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sterling.EdocIntegration.Domain.Common
{
    public class BackGroundJob
    {
        public string jobId { get; set; } = Guid.NewGuid().ToString();
        public string status { get; set; } = "Pending";
        public string description { get; set; }
        public object payload { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime? lastAttemptedAt { get; set; }
        public DateTime? nextTry { get; set; }
        public int retryCount { get; set; } = 0;
    }

}
