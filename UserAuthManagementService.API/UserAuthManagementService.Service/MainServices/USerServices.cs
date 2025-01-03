

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using UserAuthManagementService.Data.Repository.Interface;
using UserAuthManagementService.Domain.DTO.Common;
using UserAuthManagementService.Domain.DTO.Request;
using UserAuthManagementService.Domain.Entity;
using UserAuthManagementService.Service.GenericServices.Implemetation;
using UserAuthManagementService.Service.GenericServices.Interface;
using UserAuthManagementService.Service.GenericServices.Static;
using UserAuthManagementService.Service.JWtService;
using UserAuthManagementService.Service.RabbitMQServices;
using System.Security.Claims;
using System.Runtime.InteropServices;
using Nest;
using static Google.Events.Protobuf.Cloud.Batch.V1.JobNotification.Types;
using Newtonsoft.Json.Linq;
using Google.Type;
using System.Data;
using System.Net;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserAuthManagementService.Service.MainServices
{
    public class UserServices:IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly MailerService _mailerService;
        private readonly IEncryptionService _encryptionService;
        private readonly AuthService _autheService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string _cardServiceencryptkey;
        private readonly ILoggingService _loggingService;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IAuditRepository _auditRepository;


        public UserServices(IUserRepository userRepository, IAuditRepository auditRepository, ILoggingService loggingService, MailerService mailerService, IEncryptionService encryptionService, AuthService authService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _mailerService = mailerService;
            _encryptionService = encryptionService;
            _autheService = authService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _cardServiceencryptkey = "hgjhduyoipoewlkk0pkpojo4joifof4wjpjoliniubih";
            _loggingService = loggingService;
            _jwtTokenService = jwtTokenService;
            _auditRepository = auditRepository;
        }
        public async Task< GenericResponse<string> > ConfimTokenToken(ConfirmTokenRequest request, string caller,string corelationId)
        {
            caller += "||" + nameof(ConfimTokenToken);
            GenericResponse<string> response = new GenericResponse<string>();
            try
            {
                var confrimation = _jwtTokenService.ValidateToken(request.Token, caller, corelationId);
                if (confrimation != null)
                {
                    string username=confrimation.FindFirst(ClaimTypes.Name)?.Value;
                    if(username != request.username)
                    {
                        response.message = "CHECK YOUR USERNAME";
                        response.status = false;
                        response.data = string.Empty;
                        await _loggingService.LogWarning($"Token confirmation is not succesful{request.username}", caller, corelationId);
                    }
                    else
                    {
                        response.message = "success";
                        response.status = true;
                        response.data = username;
                    await _loggingService.LogInformation($"Token confirmation is succesful{request.username}", caller, corelationId);
                    }
                }
                else
                {
                    response.message = "failed";
                    response.status = false;
                    response.data = string.Empty;
                    await _loggingService.LogWarning($"Token confirmation is faile{request.username}", caller, corelationId);

                }
            }
            catch(Exception ex)
            {
                _loggingService.LogError($"Token confirmation failed", caller, ex, corelationId);
                response.message = ex.Message;
                response.status = false;
                response.data = string.Empty;
            }
            return response;
        }
        public async Task<GenericResponse<string>> AuthenticateUser(LoginRequest request, string caller,string corelationId)
        {
            try
            {
                caller += "||" + nameof(ConfimTokenToken);
                var (user, userException) = await _userRepository.GetUserByEmailUsernameAsync(request.usernameEmail);
                if (user == null||userException!=null)
                {
                    await _loggingService.LogError($"Authentication failed duet user not found {request.usernameEmail}", caller, userException, corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "user not exist",// Join error messages
                        data = string.Empty
                    };
                }
                await _loggingService.LogInformation($"user was found {request.usernameEmail}", caller,corelationId);
                string token=await _autheService.AuthenticateUser(request, user, caller,corelationId);
                await _loggingService.LogInformation($"token generated {request.usernameEmail}", caller,corelationId);
                return new GenericResponse<string>
                {
                    message = "success",
                    status = false,
                    data = token,
                };
            }
            catch (Exception ex)
            {

                await _loggingService.LogWarning($"Token generation failed is not succesful{request.usernameEmail}", caller, corelationId);
                return new GenericResponse<string>
                {
                    message = "failed",
                    status = false,
                    data = string.Empty,
                };
            }
        }

        public async Task<GenericResponse<string>> DeleteUser(string id, string caller, string corelationId)
        {
            caller += nameof(DeleteUser);
            try
            {
                if (string.IsNullOrEmpty(id)||!Guid.TryParse(id,out Guid userId))
                {
                    await _loggingService.LogWarning($"delete user failed due to id supply: {id}", caller,corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "please supply corerect Data",// Join error messages
                        data = string.Empty
                    };
                }
                else
                {
                    var (user, userExcecption) = await _userRepository.GetUserByIdAsync(userId);
                    if (user == null || userExcecption != null)
                    {
                        await _loggingService.LogWarning($"delete Not Succesfull, may be the user id does not exist before: {id}", caller, corelationId);
                        return new GenericResponse<string>
                        {
                            status = false,
                            message = "Not Succesfull, may be the user id does not exist before: ",// Join error messages
                            data = string.Empty
                        };
                    }
                    else
                    {
                        var (response, responseException) = await _userRepository.DeleteUserAsync(userId);
                        if(response>0|| responseException==null)
                        {
                            await _loggingService.LogInformation($"delete Succesfull {id}", caller, corelationId);
                            return new GenericResponse<string>
                            {
                                status = true,
                                message = "Succesfull, may be the user id does not exist before",// Join error messages
                                data = "delete succesful"
                            };
                        }
                        else
                        {
                            await _loggingService.LogInformation($"delete Not Succesfull, {responseException.Message}: {id}", caller, corelationId);
                            return new GenericResponse<string>
                            {
                                status = false,
                                message = "Not Succesfull, may be the user id does not exist before",// Join error messages
                                data = string.Empty
                            };
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogWarning($"Not Succesfull, {ex.Message}", caller, corelationId);
                return new GenericResponse<string>
                {
                    status = false,
                    message = "failed",
                    data = string.Empty
                };
            }
        }

        public async Task<GenericResponse<string>> CreateUser(UserDto createUser,  string caller, string corelationId)
        {
            caller += nameof(CreateUser);
            try
            {
                var Validate = new UserDtoValidator();
                var resp=await Validate.ValidateAsync(createUser);
                if (!resp.IsValid)
                {
                    await _loggingService.LogWarning($"create user failed due to data supply: {System.Text.Json.JsonSerializer.Serialize(createUser)}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "please supply corerect Data",// Join error messages
                        data = string.Join(", ", resp.Errors.Select(e => e.ErrorMessage)) // Join error messages
                    };
                }
                else if (createUser.role.ToLower() != "admin" && createUser.role.ToLower() != "user")
                {
                    await _loggingService.LogWarning($"create user failed due to role supply: {createUser.role}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "please supply corerect role",// Join error messages
                        data = string.Empty
                    };

                }
                else
                {
                    string pass = GeneratePassword(15);
                    createUser.Password = _encryptionService.Encrypt(createUser.Password, createUser.username + _cardServiceencryptkey);
                    var (response, responseExcpetion) = await _userRepository.AddUserAsync(createUser);
                    if (response < 1)
                    {
                        await _loggingService.LogWarning($"Not Succesfull, may be the username or the email exist before {System.Text.Json.JsonSerializer.Serialize( createUser)}", caller, corelationId);
                        return new GenericResponse<string>
                        {
                            status = false,
                            message = "Not Succesfull, may be the username or the email exist before",// Join error messages
                            data = string.Empty
                        };
                    }
                    else
                    {
                        //await _mailerService.SendEmail("Sterling Card Services", user.email, "Account creation", $"Your new password will {pass} your username is{user.username}, welcom onboard");
                        await _loggingService.LogWarning($"Adding user is Succesfull {System.Text.Json.JsonSerializer.Serialize(createUser)}", caller, corelationId);
                        return new GenericResponse<string>
                        {
                            status = true,
                            message = "succesful copy the password in the data",// Join error messages
                            data = pass
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogWarning($"Not Succesfull, failed with: {ex.Message}", caller, corelationId);
                return new GenericResponse<string>
                {
                    status = false,
                    message = "Not Succesfull, failed" ,// Join error messages
                    data = ex.Message
                };
            }
        }
        public async Task<GenericResponse<string>> UodateUser(UserDto updateUser, string caller, string corelationId)
        {
            caller += nameof(CreateUser);
            try
            {
                var Validate = new UserDtoValidator();
                var resp = await Validate.ValidateAsync(updateUser);
                if (!resp.IsValid)
                {
                    await _loggingService.LogWarning($"create user failed due to data supply: {System.Text.Json.JsonSerializer.Serialize(updateUser)}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "please supply corerect Data",// Join error messages
                        data = string.Join(", ", resp.Errors.Select(e => e.ErrorMessage)) // Join error messages
                    };
                }
                else if (updateUser.role.ToLower() != "admin" && updateUser.role.ToLower() != "user")
                {
                    await _loggingService.LogWarning($"create user failed due to role supply: {updateUser.role}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "please supply corerect role",// Join error messages
                        data = string.Empty
                    };

                }
                else
                {
                    UserDTO user = new UserDTO()
                    {
                        name=updateUser.name,
                        username=updateUser.username,
                        email=updateUser.email,
                        role=updateUser.role,
                        phoneNumber=updateUser.phoneNumber,
                        address=updateUser.address,
                        city=updateUser.city,
                        state=updateUser.state,
                        country=updateUser.country,
                        password= _encryptionService.Encrypt(updateUser.Password, updateUser.username + _cardServiceencryptkey),
                    };
                    var (response, responseExcpetion) = await _userRepository.UpdateUserAsync(user);
                    if (response < 1)
                    {
                        await _loggingService.LogWarning($"Not Succesfull, {System.Text.Json.JsonSerializer.Serialize(UodateUser)}", caller, corelationId);
                        return new GenericResponse<string>
                        {
                            status = false,
                            message = "Not Succesfull",// Join error messages
                            data = string.Empty
                        };
                    }
                    else
                    {
                        //await _mailerService.SendEmail("Sterling Card Services", user.email, "Account creation", $"Your new password will {pass} your username is{user.username}, welcom onboard");
                        await _loggingService.LogInformation($"updat user is Succesfull {System.Text.Json.JsonSerializer.Serialize(updateUser)}", caller, corelationId);
                        return new GenericResponse<string>
                        {
                            status = false,
                            message = "succesful",// Join error messages
                            data = "success"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogWarning($"Not Succesfull, failed with: {ex.Message}", caller, corelationId);
                return new GenericResponse<string>
                {
                    status = false,
                    message = "Not Succesfull, failed",// Join error messages
                    data = ex.Message
                };
            }
        }
        public async Task<GenericResponse<string>> ResetPassword(string username, string caller, string corelationId)
        {
            caller += nameof(ResetPassword);
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    await _loggingService.LogWarning($"create user failed due to data supply: {username}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "please supply corerect Data",// Join error messages
                        data = string.Empty // Join error messages
                    };
                }
                else
                {
                    var (user, userException) = await _userRepository.GetUserByEmailUsernameAsync(username);
                    if (user == null||userException!=null)
                    {
                        await _loggingService.LogError($"create user failed due to data supply: {username}", caller, userException, corelationId);
                        return new GenericResponse<string>
                        {
                            status = false,
                            message = "please supply corerect username",// Join error messages
                            data = string.Empty // Join error messages
                        };
                    }
                    else
                    {
                        string password = GeneratePassword(23);
                        user.password = _encryptionService.Encrypt(password, username + _cardServiceencryptkey);
                        string resetPasswordUrl = password;
                        var updateUser = await _userRepository.UpdateUserAsync(user);
                        await _mailerService.SendEmail(user.email, "book management Services Resetting Your Password", $"Your new password will {password}. we advice you to please change the password to your custom password.", caller, corelationId);
                        await _loggingService.LogInformation($"reset password succesful due to data supply: {username}", caller, corelationId); 
                        return new GenericResponse<string>
                        {
                            status = true,
                            message = "success",// Join error messages
                            data = "check your mail for new password" // Join error messages
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"reset password failed due to data supply: {username}", caller, ex, corelationId); 
                return new GenericResponse<string>
                {
                    status = false,
                    message = "failed",// Join error messages
                    data = string.Empty // Join error messages
                };
            }
        }

        public async Task<GenericResponse<string>> ChangePassword(string username, string oldPassword, string newPassword, string caller, string corelationId)
        {
            caller += nameof(ChangePassword);
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
                {
                    await _loggingService.LogWarning($"please supply corerect username and password: {username}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "failed",// Join error messages
                        data = "please supply corerect username and password"// Join error messages
                    };
                }
                else
                {
                    var (user, userException) = await _userRepository.GetUserByEmailUsernameAsync(username);
                    if (user == null)
                    {
                        await _loggingService.LogWarning($"please supply corerect username and password: {username}", caller, corelationId);
                        return new GenericResponse<string>
                        {
                            status = false,
                            message = "failed",// Join error messages
                            data = "please supply corerect username and password"// Join error messages
                        };
                    }
                    else
                    {
                        var decyptPassword = _encryptionService.Decrypt(user.password, user.username + _cardServiceencryptkey);
                        if (decyptPassword == oldPassword)
                        {
                            user.password = _encryptionService.Encrypt(newPassword, username + _cardServiceencryptkey);
                            //await _mailerService.SendEmail("Sterling Card Services", user.email, "Resetting Your Password", $"Your password has been changed succesfully. we advice you toplease change the password to your custom password and thsis will expirei the next 2 hours");
                            var response = await _userRepository.UpdateUserAsync(user);
                            await _loggingService.LogInformation($"changing password is succesful for: {username}", caller, corelationId);
                            return new GenericResponse<string>
                            {
                                status = true,
                                message = "success",// Join error messages
                                data = "succesful"// Join error messages
                            };
                        }
                        else
                        {
                            await _loggingService.LogWarning($"changing password is not succesful for: {username} because of incorerect old password", caller, corelationId);
                            return new GenericResponse<string>
                            {
                                status = true,
                                message = "incorrect old password",// Join error messages
                                data = string.Empty,// Join error messages
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogWarning($"changing password failed for : {username} because of incorerect old password", caller, corelationId);
                return new GenericResponse<string>
                {
                    status = true,
                    message = "failed",// Join error messages
                    data = string.Empty,// Join error messages
                };
            }
        }

        public async Task<GenericResponse<UserResponseDto>> GetUserByUsername(string username, string caller, string corelationId)
        {
            caller += nameof(GetUserByUsername);
            try
            {
                var response= (await _userRepository.GetUserByEmailUsernameAsync(username));
                if(response.Item1==null||response.Item2!=null)
                {
                    await _loggingService.LogInformation($"Gettinf user  is not succesful for: {username} ", caller, corelationId);
                    return new GenericResponse<UserResponseDto>
                    {
                        status = false,
                        message = "user not found",// Join error messages
                        data =default( UserResponseDto)
                    };
                }
                else
                {
                    await _loggingService.LogInformation($"Gettinf user  is succesful for: {username} ", caller, corelationId);
                    return new GenericResponse<UserResponseDto>
                    {
                        status = true,
                        message = "use found",// Join error messages
                        data =new UserResponseDto()
                        {
                            name = response.Item1.name,
                            username = response.Item1.username,
                            email = response.Item1.email,
                            role = response.Item1.role,
                            phoneNumber = response.Item1.phoneNumber,
                            address = response.Item1.address,
                            city = response.Item1.city,
                            state = response.Item1.state,
                            country = response.Item1.country,
                            balance = response.Item1.balance,
                            Id = response.Item1.id
                        },// Join error messages,// Join error messages
                    };
                }

            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"Gettinf user  is failed for: {username} ", caller, ex, corelationId);
                return new GenericResponse<UserResponseDto>
                {
                    status = true,
                    message = "use found",// Join error messages
                    data = null,// Join error messages
                };
            }
        }
        public async Task<GenericResponse<string>> AuditUser(AuditDTO request, string caller, string corelationId)
        {
            try
            {
                caller += "||" + nameof(AuditUser);
                var (user, userException) = await _userRepository.GetUserByEmailUsernameAsync(request.userEmail);
                if (user == null || userException != null)
                {
                    await _loggingService.LogError($"AuditUser failed duet user not found {request.userEmail}", caller, userException, corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "user not exist",// Join error messages
                        data = string.Empty
                    };
                }
                var audit = new Audit()
                {
                    ip=request.ip,
                    correlationId=request.correlationId.ToString(),
                    type=request.type,
                    description =request.description,
                     user =user.id.ToString(),
                };
                await _loggingService.LogInformation($"user was found and audit class was upload {request.userEmail}", caller, corelationId);
                var (resp, respExcpetion) = await _auditRepository.CreateAudit(audit);
                if (respExcpetion != null||resp==null)
                {
                    await _loggingService.LogWarning($"audit insertion failed {System.Text.Json.JsonSerializer.Serialize(request)}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        message = "failed",
                        status = false,
                        data = string.Empty,
                    };
                }
                else
                {
                    await _loggingService.LogInformation($"audit insertion succesful {System.Text.Json.JsonSerializer.Serialize(request)}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        message = "success",
                        status = true,
                        data = audit.auditId.ToString(),
                    };
                }

            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"audit insertion failed is not succesful{System.Text.Json.JsonSerializer.Serialize(request)}", caller,ex, corelationId);
                return new GenericResponse<string>
                {
                    message = "failed",
                    status = false,
                    data = string.Empty,
                };
            }
        }
        public async Task<GenericResponse<string>> ChangeBalanceUser(string username, double amount, string caller, string corelationId)
        {
            try
            {
                caller += "||" + nameof(ChangeBalanceUser);
                var (user, userException) = await _userRepository.GetUserByEmailUsernameAsync(username);
                if (user == null || userException != null)
                {
                    await _loggingService.LogError($"AuditUser failed duet user not found {username}", caller, userException, corelationId);
                    return new GenericResponse<string>
                    {
                        status = false,
                        message = "user not exist",// Join error messages
                        data = string.Empty
                    };
                }
                var audit = new AuditDTO()
                {
                    correlationId = Guid.Parse(corelationId),
                    type = "balanceUpdate",
                    description = $"Attempting to change Balance of{username} by adding{amount}",
                    userEmail =username,
                };
               await AuditUser(audit, caller, corelationId);
                await _loggingService.LogInformation($"user was found and audit class was upload at attemp{username}", caller, corelationId);
                user.balance = user.balance+ amount;
                var (resp, respExcpetion) = await _userRepository.UpdateUserAsync(user);
                if (respExcpetion != null || resp == null)
                {
                    await _loggingService.LogWarning($"change Balance of{username} by adding{amount}failed {username}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        message = "failed",
                        status = false,
                        data = string.Empty,
                    };
                }
                else
                {
                    await _loggingService.LogInformation($"change Balance of{username} by adding{amount}succesful {username}", caller, corelationId);
                    return new GenericResponse<string>
                    {
                        message = "success",
                        status = false,
                        data = $"{amount} debited from {username} account",
                    };
                }

            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"change Balance of{username} by adding{amount}failed {username}", caller, ex, corelationId);
                return new GenericResponse<string>
                {
                    message = "failed",
                    status = false,
                    data = string.Empty,
                };
            }
        }
        public string GeneratePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }


}
//using UserAuthManagementService.Service.RabbitMQServices;

//var requestPublish = new RequestPublish(bus);
//await requestPublish.SendRequestMessageAsync("ProcessBook", bookPayload, "service-b-queue");