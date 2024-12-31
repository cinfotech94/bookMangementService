using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UserAuthManagementService.Domain.DTO.Request;
using UserAuthManagementService.Service.GenericServices.Interface;
using UserAuthManagementService.Domain.DTO.Common;
using UserAuthManagementService.Data.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using UserAuthManagementService.Domain.Entity;
using UserAuthManagementService.ServiceGeneric.Services.Interface;


namespace UserAuthManagementService.Service.JWtService
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _cardServiceencryptkey;
        private readonly IConfiguration _configuration;
        private readonly ILoggingService _loggingService;
        public AuthService(IServiceProvider serviceProvider,IConfiguration configuration, ILoggingService loggingService)
        {
            _serviceProvider = serviceProvider;
            _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
            _jwtTokenService = _serviceProvider.GetRequiredService<JwtTokenService>();
            _encryptionService = _serviceProvider.GetRequiredService<IEncryptionService>();
            _configuration = configuration;
            _cardServiceencryptkey = "hgjhduyoipoewlkk0pkpojo4joifof4wjpjoliniubih";
            _loggingService = loggingService;
        }
        public async Task<string> AuthenticateUser(LoginRequest loginRequest,UserDTO user, string caller, string corelationId)
        {
            try
            {
                caller = caller + "||" + nameof(AuthenticateUser);

                // Get user from the database
                bool check = await VerifyPasswordHash(loginRequest.password, user.password,user.username, caller, corelationId);
                if (user == null || !(check))
                    {
                           await _loggingService.LogWarning($"Authentication failedfor {System.Text.Json.JsonSerializer.Serialize(loginRequest) }", caller, corelationId);

                            return null; // Authentication failed

                    }

                // Authentication successful, generate JWT token
                await _loggingService.LogInformation($"TokenGotSuccesful for { System.Text.Json.JsonSerializer.Serialize(loginRequest) }", caller, corelationId);

                return _jwtTokenService.GenerateJwtToken(user, caller, corelationId);
            }
            catch(Exception ex)
            {
                await _loggingService.LogError($"Token Request is not Succesfulfor {System.Text.Json.JsonSerializer.Serialize(loginRequest) }", caller, ex, corelationId);

return null; // Authentication failed
            }
        }        
        private async Task<bool> VerifyPasswordHash(string password, string storedHash,string username,string caller, string corelationId)
{
            try
            {
                caller = caller + "||" + nameof(VerifyPasswordHash);

                // Assuming storedHash is a base64-encoded hash of the password
                var hashPassword = _encryptionService.Decrypt(storedHash, username+_cardServiceencryptkey);
              await  _loggingService.LogInformation($"VerifyPasswordHash is not succesful for {username}", caller, corelationId);

        return password == hashPassword;
            }
            catch (Exception ex)
            {
               await _loggingService.LogError($"VerifyPasswordHash is failed for {username}", caller, ex, corelationId);
        return false;
            }
            
        }
    }

}
