using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Domain.DTO.Common
{
    public class RequestMessage
    {
        public string MethodName { get; set; } = string.Empty;
        public string caller { get; set; }
        public object? Payload { get; set; }
        public DateTime RequestTime { get; set; } = DateTime.UtcNow;
    }

    public class ResponseMessage
    {
        public string caller { get; set; }
        public string MethodName { get; set; } = string.Empty;
        public object? Payload { get; set; }
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    }

}
