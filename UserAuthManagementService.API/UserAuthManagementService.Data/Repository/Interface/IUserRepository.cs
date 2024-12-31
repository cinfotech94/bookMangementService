using UserAuthManagementService.Domain.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthManagementService.Domain.Entity;

namespace UserAuthManagementService.Data.Repository.Interface
{
    public interface IUserRepository
    {
        Task<(int, Exception)> AddUserAsync(UserDto user);
        Task<(UserDto, Exception)> GetUserByIdAsync(Guid userId);
        Task<(int, Exception)> UpdateUserAsync(User user);
        Task<(int, Exception)> DeleteUserAsync(Guid userId);
        Task<(User, Exception)> GetUserByEmailUsernameAsync(string emailUsername);
    }
}
