using static System.Runtime.InteropServices.JavaScript.JSType;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Domain.DTO.Common;
    public class Result<T>
    {
        public T Content { get; set; }
        public Error? Error { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; } = "";
        public string Message { get; set; } = "";
        public string RequestId { get; set; } = "";
        public string ResponseCode { get; set; } = "";
        public string ResponseDescription { get; set; } = "";
        public bool IsSuccess { get; set; } = true;
        public DateTime RequestTime { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow.AddHours(1);
        public OutboundLog OutboundLog { get; set; }
    }

