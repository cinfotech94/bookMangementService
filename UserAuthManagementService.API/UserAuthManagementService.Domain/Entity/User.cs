using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthManagementService.Domain.Entity
{
    public class UserDTO
    {
        [Key]
        public Guid id { get; set; }= Guid.NewGuid();
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public double balance { get; set; } = 0;
        public string password { get; set; }

    }
}
