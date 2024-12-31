using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UserAuthManagementService.Data.Repository.Interface;
using UserAuthManagementService.Domain.DTO.Common;
using UserAuthManagementService.Service.GenericServices.Implemetation;
using UserAuthManagementService.Service.GenericServices.Interface;

namespace UserAuthManagementService.ServiceGeneric.Services.Interface;
public class LoggingService : ILoggingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DateTime requestDate;
    private readonly IServiceProvider _serviceProvider;
    public LoggingService(IHttpContextAccessor httpContextAccessor,IServiceProvider serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
        requestDate = DateTime.UtcNow;

    }
    // Logs informational messages
    public async Task<int> LogInformation(string description, string caller,string corelationId, [CallerMemberName] string methodName = "")
    {
        var ip=GetIp();
        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Information("User:{User},Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description}, corelationId:{corelationId}",
user, methodName, caller, DateTime.UtcNow, description, corelationId);
        }
        else
        {
            Log.Information("Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description,corelationId:{corelationId}",
methodName, caller, DateTime.UtcNow, description,corelationId);

        }
        return 1;
    }

    // Logs warning messages
    public async Task<int> LogWarning(string description, string caller,string corelationId, [CallerMemberName] string methodName = "")
    {
       

        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Warning("User:{User},Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description} ,corelationId{corelationId}",
            user, methodName, caller, DateTime.UtcNow, description, corelationId);
        }
        else
        {
            Log.Warning("Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description}, corelationId:{corelationId}",
      methodName, caller, DateTime.UtcNow, description);
        }
        return 1;
    }

    // Logs errors with exception details
    public async Task<int> LogError(string description, string caller, Exception ex,string corelationId, [CallerMemberName] string methodName = "")
    {
       

        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Error(ex, "Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description},User:{User}, corelationId:{corelationId}",
            methodName, caller, DateTime.UtcNow, description, user, corelationId);
        }
        else
        {
            Log.Error(ex, "Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description},corelationId:{corelationId}",
methodName, caller, DateTime.UtcNow, description, corelationId);
        }
        return 1;
    }

    // Logs debug messages
    public async Task<int> LogDebug(string description, string caller,string corelationId, [CallerMemberName] string methodName = "")
    {
       

        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Debug("User:{User},Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description}, corelationId:{corelationId}",
            user, methodName, caller, DateTime.UtcNow, description, corelationId);
        }
        else
        {
            Log.Debug("Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description}, corelationId:{corelationId}",
methodName, caller, DateTime.UtcNow, description, corelationId);
        }
        return 1;
    }

    // Logs fatal errors with exception details
    public async Task<int> LogFatal(string description, string caller, Exception ex, string corelationId, [CallerMemberName] string methodName = "")
    {

       

        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Fatal(ex, "Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description},User:{User}, corelationId:{corelationId}",
            methodName, caller, DateTime.UtcNow, description, user);

        }
        else
        {
            Log.Fatal(ex, "Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description},corelationId:{corelationId}",
methodName, caller, DateTime.UtcNow, description,corelationId);
        }
        return 1;
    }

    // Logs detailed API call information using APILog object
    public async Task<int> LogAPICall(APILog apiLog, string caller,string corelationId, [CallerMemberName] string methodName = "")
    {
       

        var user = _httpContextAccessor.HttpContext?.User;
        var message = "";
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Information(
            "User:{user},System Called: {SystemCalledName},Caller:{Caller}, API Called: {APICalled}, Method: {APIMethod}, Log Date: {LogDate}, " +
            "Request DateTime: {RequestDateTime}, Request Details: {RequestDetails}, Response DateTime: {ResponseDateTime}, " +
            "Response Details: {ResponseDetails}, Exception: {ExceptionDetails}, Method: {MethodName}, corelationId:{corelationId}, corelationId:{corelationId}",
            user, apiLog.SystemCalledName, caller, apiLog.APICalled, apiLog.APIMethod, apiLog.LogDate, apiLog.RequestDateTime,
            apiLog.RequestDetails, apiLog.ResponseDateTime, apiLog.ResponseDetails, apiLog.ExceptionDetails ?? "None", methodName, corelationId
        );
        }
        else
        {
            Log.Information(
            "System Called: {SystemCalledName},Caller:{Caller}, API Called: {APICalled}, Method: {APIMethod}, Log Date: {LogDate}, " +
            "Request DateTime: {RequestDateTime}, Request Details: {RequestDetails}, Response DateTime: {ResponseDateTime}, " +
            "Response Details: {ResponseDetails}, Exception: {ExceptionDetails}, Method: {MethodName,corelationId:{corelationId}}",
            apiLog.SystemCalledName, caller, apiLog.APICalled, apiLog.APIMethod, apiLog.LogDate, apiLog.RequestDateTime,
            apiLog.RequestDetails, apiLog.ResponseDateTime, apiLog.ResponseDetails, apiLog.ExceptionDetails ?? "None", methodName, corelationId
            );
         
        }

        return 1;
    }
    private  string GetIp()
    {
        var context = _httpContextAccessor.HttpContext;
        string ip = string.Empty;
            ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault(); ;

            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress?.ToString();
            }
            var userHostAddress = context.Connection.RemoteIpAddress?.ToString();

            ip += "|" + userHostAddress;
        return ip;
    }
}

