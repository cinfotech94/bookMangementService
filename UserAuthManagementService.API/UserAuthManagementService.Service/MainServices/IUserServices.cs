using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserAuthManagementService.Domain.DTO.Common;
using UserAuthManagementService.Domain.DTO.Request;
using UserAuthManagementService.Domain.Entity;

namespace UserAuthManagementService.Service.MainServices
{
    public interface IUserServices
    {
        Task<GenericResponse<string>> ChangeBalanceUser(string username, double amount, string caller, string corelationId);
        Task<GenericResponse<string>> AuditUser(AuditDTO request, string caller, string corelationId);
        Task<GenericResponse<UserResponseDto>> GetUserByUsername(string username, string caller, string corelationId);
        Task<GenericResponse<string>> ChangePassword(string username, string oldPassword, string newPassword, string caller, string corelationId);
        Task<GenericResponse<string>> ResetPassword(string username, string caller, string corelationId);
        Task<GenericResponse<string>> UodateUser(UserDto updateUser, string caller, string corelationId);
        Task<GenericResponse<string>> CreateUser(UserDto createUser, string caller, string corelationId);
        Task<GenericResponse<string>> DeleteUser(string id, string caller, string corelationId);
        Task<GenericResponse<string>> AuthenticateUser(LoginRequest request, string caller, string corelationId);
        Task<GenericResponse<ClaimsPrincipal>> ConfimTokenToken(ConfirmTokenRequest request, string caller, string corelationId);
    }
}
