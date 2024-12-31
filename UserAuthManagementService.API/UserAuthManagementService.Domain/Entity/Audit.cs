using UserAuthManagementService.Domain.Entity;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Claims;

namespace UserAuthManagementService.Domain.Entity;
public class Audit
{
    public Guid auditId { get; set; } = Guid.NewGuid();
    public string? ip { get; set; }
    public Guid correlationId { get; set; }
    public string type { get; set; }
    public string description { get; set; }
    public DateTime happenOn { get; set; } = DateTime.Now;
    public string? user { get;  set; }


}
