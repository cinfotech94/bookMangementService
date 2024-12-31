
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Concurrent;

namespace PaymentPurchaseManagementService.Domain.DTO.Common;

public partial class InboundLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId InboundLogId { get; set; }
    public string? Ip { get; set; }
    public string? CorrelationId { get; set; }
    public string? InstitutionId { get; set; }
    public string? Level { get; set; }
    public string? RequestSystem { get; set; }
    public string? APICalled { get; set; }
    public string? APIMethod { get; set; }
    public string? ImpactUniqueIdentifier { get; set; }
    public string? ImpactUniqueidentifierValue { get; set; }
    public string? AlternateUniqueIdentifier { get; set; }
    public string? AlternateUniqueidentifierValue { get; set; }
    public string? RequestDetails { get; set; }
    public DateTime RequestDateTime { get; set; }
    public string? ResponseDetails { get; set; }
    public DateTime ResponseDateTime { get; set; }
    public ConcurrentBag<OutboundLog>? OutboundLogs { get; set; }
    public string? ExceptionDetails { get; set; }
    public InboundLog()
    {
        InboundLogId = ObjectId.GenerateNewId();
    }
}
