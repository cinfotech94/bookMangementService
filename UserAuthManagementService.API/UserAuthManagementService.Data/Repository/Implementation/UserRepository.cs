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
                GetCachedData();
                var query = @"INSERT INTO users (id, name, email, role, phoneNumber, address, city, state, country, balance, password)
                          VALUES (@Id, @Name, @Email, @Role, @PhoneNumber, @Address, @City, @State, @Country, @Balance, @Password)";
                using (var connection = _context.CreateConnection())
                {
                    int response = await connection.ExecuteAsync(query, new
                    {
                        Id = Guid.NewGuid(),
                        user.name,
                        user.email,
                        user.role,
                        user.phoneNumber,
                        user.address,
                        user.city,
                        user.state,
                        user.country,
                        user.balance,
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
                GetCachedData();
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
        public async Task<(User, Exception)> GetUserByEmailUsernameAsync(string emailUsername)
        {
            try
            {
                GetCachedData();
                var query = @"SELECT id, name, email, role, phoneNumber, address, city, state, country, balance, password
                          FROM users
                          WHERE email = @UserId or username=@UserId";
                using (var connection = _context.CreateConnection())
                {
                    var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { UserId = emailUsername });
                    return (user, null);
                }
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        // Update user details
        public async Task<(int, Exception)> UpdateUserAsync(User user)
        {
            try
            {
                GetCachedData();
                var query = @"UPDATE users
                          SET name = @Name, email = @Email, role = @Role, phoneNumber = @PhoneNumber,
                              address = @Address, city = @City, state = @State, country = @Country,
                              balance = @Balance, password = @Password
                          WHERE id = @Id";
                using (var connection = _context.CreateConnection())
                {
                    int response = await connection.ExecuteAsync(query, new
                    {
                        user.id,
                        user.name,
                        user.email,
                        user.role,
                        user.phoneNumber,
                        user.address,
                        user.city,
                        user.state,
                        user.country,
                        user.balance,
                        user.password
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
                GetCachedData();
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
        private async Task GetCachedData()
        {
            string cacheKey = "databaseExist";
            if (!_memoryCache.TryGetValue(cacheKey, out string cachedData))
            {
                // Data not in cache, fetch it
                try
                {
                    using (var connection = _context.CreateConnection())
                    {
                        connection.Open();

                        // Step 1: Create Users table if it does not exist
                        var createUsersTableQuery = @"
                            DO $$
                            BEGIN
                                IF NOT EXISTS (
                                    SELECT 1 
                                    FROM information_schema.tables 
                                    WHERE table_name = 'users' AND table_schema = 'public'
                                ) THEN
                                    CREATE TABLE Users (
                                        Id UUID PRIMARY KEY,
                                        Name VARCHAR(255),
                                        Username VARCHAR(255),
                                        Email VARCHAR(255) UNIQUE,
                                        Role VARCHAR(255),
                                        PhoneNumber VARCHAR(15),
                                        Address VARCHAR(500),
                                        City VARCHAR(255),
                                        State VARCHAR(255),
                                        Country VARCHAR(255),
                                        Balance DOUBLE PRECISION DEFAULT 0,
                                        Password VARCHAR(255)
                                    );
                                END IF;
                            END $$;";

                        await connection.ExecuteAsync(createUsersTableQuery);
                        //Console.WriteLine("Users table checked and created if necessary.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
                }

                cachedData = "yes";

                // Set cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(365)) // Refresh expiration after each access
                    .SetAbsoluteExpiration(TimeSpan.FromDays(500)); // Cache expires after 500 days

                // Save data in cache
                _memoryCache.Set(cacheKey, cachedData, cacheEntryOptions);
            }
        }

    }

}
