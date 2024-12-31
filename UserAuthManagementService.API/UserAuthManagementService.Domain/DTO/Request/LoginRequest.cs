using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthManagementService.Domain.DTO.Request
{
    public class LoginRequest
    {
        public string usernameEmail {  get; set; }
        public string password {  get; set; }
    }
    public class ConfirmTokenRequest
    {
        public string username {  get; set; }
        public string Token {  get; set; }
    }
}
