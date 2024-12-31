using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Domain.DTO.Common
{
    public class MailRequest
    {
        public string sourceEmail { get; set; }
        public string destinationEmail { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }

}
