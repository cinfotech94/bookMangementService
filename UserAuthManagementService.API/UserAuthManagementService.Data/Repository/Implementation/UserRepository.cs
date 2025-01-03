using Dapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthManagementService.Data.context;
using UserAuthManagementService.Data.Repository.Interface;
using UserAuthManagementService.Domain.DTO.Request;
using UserAuthManagementService.Domain.Entity;

namespace UserAuthManagementService.Data.Repository.Implementation
{
    public class UserRepository: IUserRepository
    {
        private readonly DappperDbConnection _context;
        private readonly IMemoryCache _memoryCache;

        public UserRepository(DappperDbConnection context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        // Add a new user
        public async Task<(int, Exception)> AddUserAsync(UserDto user)
        {
            try
            {
                
                var query = @"INSERT INTO users (id, name,email,username, role, phoneNumber, address, city, state, country, password)
                          VALUES (@Id, @Name, @Email, @Username, @Role, @PhoneNumber, @Address, @City, @State, @Country, @Password)";
                using (var connection = _context.CreateConnection())
                {
                    int response = await connection.ExecuteAsync(query, new
                    {
                        Id = Guid.NewGuid(),
                        user.name,
                        user.email,
                        user.username,
                        user.role,
                        user.phoneNumber,
                        user.address,
                        user.city,
                        user.state,
                        user.country,
                        user.Password
                    });
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (0, ex);
            }
        }

        // Get user by id
        public async Task<(UserDto, Exception)> GetUserByIdAsync(Guid userId)
        {
            try
            {
                
                var query = @"SELECT id, name, email, role,username, phoneNumber, address, city, state, country, balance, password
                          FROM users
                          WHERE id = @UserId";
                using (var connection = _context.CreateConnection())
                {
                    var user = await connection.QueryFirstOrDefaultAsync<UserDto>(query, new { UserId = userId });
                    return (user, null);
                }
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }
        public async Task<(UserDTO, Exception)> GetUserByEmailUsernameAsync(string emailUsername)
        {
            try
            {
                
                var query = @"SELECT id,username, name, email, role, phoneNumber, address, city, state, country, balance, password
                          FROM users
                          WHERE email = @UserId or username=@UserId";
                using (var connection = _context.CreateConnection())
                {
                    var user = await connection.QueryFirstOrDefaultAsync<UserDTO>(query, new { UserId = emailUsername });
                    return (user, null);
                }
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        // Update user details
        public async Task<(int, Exception)> UpdateUserAsync(UserDTO user)
        {
            try
            {
                
                var query = @"UPDATE users
                          SET name = @Name,username = @Username, email = @Email, role = @Role, phoneNumber = @PhoneNumber,
                              address = @Address, city = @City, state = @State, country = @Country,
                              balance = @Balance, password = @Password
                          WHERE username = @Username";
                using (var connection = _context.CreateConnection())
                {
                    int response = await connection.ExecuteAsync(query, new
                    {
                        Name=user.name,
                        Email=user.email,
                        Username=user.username,
                        Role = user.role,
                        PhoneNumber = user.phoneNumber,
                        Address=user.address,
                        City=user.city,
                        State = user.state,
                        Country=user.country,
                        Balance=user.balance,
                        Password=user.password
                    });
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (0, ex);
            }
        
        }

        // Delete a user
        public async Task<(int, Exception)> DeleteUserAsync(Guid userId)
        {
            try
            {
                
                var query = @"DELETE FROM users WHERE id = @UserId";
                using (var connection = _context.CreateConnection())
                {
                    int response = await connection.ExecuteAsync(query, new { UserId = userId });
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (0, ex);
            }
        }
    }

}
