using BookManagementService.Domain.Entity;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Claims;
namespace BookManagementService.Domain.DTO.Request;

public class AuditDTO
{
    public string? ip { get; private set; }
    public Guid correlationId { get; set; }
    public string Id { get; set; }
    public string type { get; set; }
    public string description { get; set; }
    public DateTime happenOn { get; private set; } = DateTime.Now;
    public string? user { get; private set; }

    public void PopulateAuditData(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext), "HttpContext is null.");
        }

        InitializeIp(httpContextAccessor);
        InitializeUser(httpContextAccessor);
        happenOn = DateTime.Now;
    }

    private void InitializeIp(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        ip = context?.Request.Headers["X-Forwarded-For"].FirstOrDefault()
             ?? context?.Connection.RemoteIpAddress?.ToString()
             ?? "Unknown IP";
    }

    private void InitializeUser(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        var userClaims = context?.User;
        user = userClaims?.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown User";
    }
}
