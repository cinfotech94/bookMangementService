using UserAuthManagementService.Domain.Entity;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Claims;
using MongoDB.Bson;

namespace UserAuthManagementService.Domain.Entity;
public class Audit
{
    public ObjectId auditId { get; set; } = ObjectId.GenerateNewId();
    public string? ip { get; set; }
    public string correlationId { get; set; }
    public string from { get; set; }
    public string type { get; set; }
    public string description { get; set; }
    public DateTime happenOn { get; set; } = DateTime.Now;
    public string? user { get;  set; }


}
