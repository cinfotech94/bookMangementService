using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Domain.DTO.Common
{
    public class Content
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
    }

    public class MailResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public Content content { get; set; }
        public bool hasError { get; set; }
        public string errorMessage { get; set; }
        public object error { get; set; }
        public string requestId { get; set; }
        public DateTime requestTime { get; set; }
        public DateTime responseTime { get; set; }
    }

}
