using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookManagementService.Domain.DTO.Common;

namespace BookManagementService.Service.GenericServices.Interface;
public interface ILoggingService
{
    Task<int> LogAPICall(APILog apiLog, string caller, [CallerMemberName] string methodName = "");
    Task<int> LogFatal(string description, string caller, Exception ex, [CallerMemberName] string methodName = "");
    Task<int> LogDebug(string description, string caller, [CallerMemberName] string methodName = "");
    Task<int> LogError(string description, string caller, Exception ex, [CallerMemberName] string methodName = "");
        Task<int> LogWarning(string description, string caller, [CallerMemberName] string methodName = "");
    Task<int> LogInformation(string description, string caller, [CallerMemberName] string methodName = "");

}
