using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthManagementService.Domain.DTO.Request;
using UserAuthManagementService.Service.MainServices;

namespace UserAuthManagementService.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserAuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpPut("ConfimTokenToken")]
        public async Task<IActionResult> ConfimTokenToken(ConfirmTokenRequest confirmTokenRequest)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.ConfimTokenToken(confirmTokenRequest, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPost("GenerateToken")]
        public async Task<IActionResult> AuthenticateUser(LoginRequest loginRequest)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.AuthenticateUser(loginRequest, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.DeleteUser(id, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDto createUser)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.CreateUser(createUser, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UodateUser(UserDto updateUser)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.UodateUser(updateUser, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string username)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.ResetPassword(username, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string username,  string oldPassword, string newPassword)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.ChangePassword(username, oldPassword, newPassword, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPut("GetUserByUsername")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.GetUserByUsername(username, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPost("AuditUser")]
        public async Task<IActionResult> AuditUser(AuditDTO auditDTO)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.AuditUser(auditDTO, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPut("AuditUser")]
        public async Task<IActionResult> ChangeBalanceUser(string username, double amount)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _userServices.ChangeBalanceUser(username, amount, nameof(UserAuthController), correlationId.ToString());
            return Ok(response);
        }
    }
}
