
using PaymentPurchaseManagementService.Service.GenericServices.Interface;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace PaymentPurchaseManagementService.Service.GenericServices.Implemetation;
public class IpService:IIpService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILoggingService _logger;
    private readonly IServiceProvider _serviceProvider;

    public IpService(IServiceProvider serviceProvider, ILoggingService logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _httpContextAccessor = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IHttpContextAccessor>();
    }
    public string GetIp()
    {
        var context = _httpContextAccessor.HttpContext;
        string ip = string.Empty;
        try
        {
            ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault(); ;

            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress?.ToString();
            }
            var userHostAddress = context.Connection.RemoteIpAddress?.ToString();

            ip += "|" + userHostAddress;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString(),nameof(GetIp),ex, Guid.NewGuid().ToString());
        }
        return ip;
    }
}

