using UserAuthManagementService.Domain.Entity;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Claims;
namespace UserAuthManagementService.Domain.DTO.Request;

public class AuditDTO
{
    public string? ip { get; private set; }
    public Guid correlationId { get; set; }
    public string type { get; set; }
    public string description { get; set; }
    public DateTime happenOn { get; private set; } = DateTime.Now;
    public string userEmail { get;  set; }

    public void PopulateAuditData(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext), "HttpContext is null.");
        }

        InitializeIp(httpContextAccessor);
        happenOn = DateTime.Now;
    }

    private void InitializeIp(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        ip = context?.Request.Headers["X-Forwarded-For"].FirstOrDefault()
             ?? context?.Connection.RemoteIpAddress?.ToString()
             ?? "Unknown IP";
    }
}
