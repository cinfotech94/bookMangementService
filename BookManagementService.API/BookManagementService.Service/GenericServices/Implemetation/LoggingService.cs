using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using BookManagementService.Domain.DTO.Common;
using BookManagementService.Service.GenericServices.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using BookManagementService.Data.Repository.Interface;
using BookManagementService.Domain.DTO.Common;
using BookManagementService.Service.GenericServices.Implemetation;
using BookManagementService.Service.GenericServices.Interface;

namespace BookManagementService.ServiceGeneric.Services.Interface;
public class LoggingService : ILoggingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DateTime requestDate;
    private readonly IServiceProvider _serviceProvider;
    public LoggingService(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
        requestDate = DateTime.UtcNow;

    }
    // Logs informational messages
    public async Task<int> LogInformation(string description, string caller, string correlationId, [CallerMemberName] string methodName = "")
    {
        var ip = GetIp();
        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Information("User:{User},Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description}, correlationId:{correlationId}",
user, methodName, caller, DateTime.UtcNow, description, correlationId);
        }
        else
        {
            Log.Information("Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description,correlationId:{correlationId}",
methodName, caller, DateTime.UtcNow, description, correlationId);

        }
        return 1;
    }

    // Logs warning messages
    public async Task<int> LogWarning(string description, string caller, string correlationId, [CallerMemberName] string methodName = "")
    {


        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Warning("User:{User},Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description} ,correlationId{correlationId}",
            user, methodName, caller, DateTime.UtcNow, description, correlationId);
        }
        else
        {
            Log.Warning("Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description}, correlationId:{correlationId}",
      methodName, caller, DateTime.UtcNow, description);
        }
        return 1;
    }

    // Logs errors with exception details
    public async Task<int> LogError(string description, string caller, Exception ex, string correlationId, [CallerMemberName] string methodName = "")
    {


        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Error(ex, "Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description},User:{User}, correlationId:{correlationId}",
            methodName, caller, DateTime.UtcNow, description, user, correlationId);
        }
        else
        {
            Log.Error(ex, "Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description},correlationId:{correlationId}",
methodName, caller, DateTime.UtcNow, description, correlationId);
        }
        return 1;
    }

    // Logs debug messages
    public async Task<int> LogDebug(string description, string caller, string correlationId, [CallerMemberName] string methodName = "")
    {


        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Debug("User:{User},Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description}, correlationId:{correlationId}",
            user, methodName, caller, DateTime.UtcNow, description, correlationId);
        }
        else
        {
            Log.Debug("Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description}, correlationId:{correlationId}",
methodName, caller, DateTime.UtcNow, description, correlationId);
        }
        return 1;
    }

    // Logs fatal errors with exception details
    public async Task<int> LogFatal(string description, string caller, Exception ex, string correlationId, [CallerMemberName] string methodName = "")
    {



        var user = _httpContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Fatal(ex, "Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description},User:{User}, correlationId:{correlationId}",
            methodName, caller, DateTime.UtcNow, description, user);

        }
        else
        {
            Log.Fatal(ex, "Method: {MethodName},Caller:{Caller}, DateTime: {DateTime}, Description: {Description},correlationId:{correlationId}",
methodName, caller, DateTime.UtcNow, description, correlationId);
        }
        return 1;
    }

    // Logs detailed API call information using APILog object
    public async Task<int> LogAPICall(APILog apiLog, string caller, string correlationId, [CallerMemberName] string methodName = "")
    {


        var user = _httpContextAccessor.HttpContext?.User;
        var message = "";
        if (user.Identity?.IsAuthenticated == true)
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            Log.Information(
            "User:{user},System Called: {SystemCalledName},Caller:{Caller}, API Called: {APICalled}, Method: {APIMethod}, Log Date: {LogDate}, " +
            "Request DateTime: {RequestDateTime}, Request Details: {RequestDetails}, Response DateTime: {ResponseDateTime}, " +
            "Response Details: {ResponseDetails}, Exception: {ExceptionDetails}, Method: {MethodName}, correlationId:{correlationId}, correlationId:{correlationId}",
            user, apiLog.SystemCalledName, caller, apiLog.APICalled, apiLog.APIMethod, apiLog.LogDate, apiLog.RequestDateTime,
            apiLog.RequestDetails, apiLog.ResponseDateTime, apiLog.ResponseDetails, apiLog.ExceptionDetails ?? "None", methodName, correlationId
        );
        }
        else
        {
            Log.Information(
            "System Called: {SystemCalledName},Caller:{Caller}, API Called: {APICalled}, Method: {APIMethod}, Log Date: {LogDate}, " +
            "Request DateTime: {RequestDateTime}, Request Details: {RequestDetails}, Response DateTime: {ResponseDateTime}, " +
            "Response Details: {ResponseDetails}, Exception: {ExceptionDetails}, Method: {MethodName,correlationId:{correlationId}}",
            apiLog.SystemCalledName, caller, apiLog.APICalled, apiLog.APIMethod, apiLog.LogDate, apiLog.RequestDateTime,
            apiLog.RequestDetails, apiLog.ResponseDateTime, apiLog.ResponseDetails, apiLog.ExceptionDetails ?? "None", methodName, correlationId
            );

        }

        return 1;
    }
    private string GetIp()
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

