
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentPurchaseManagementService.Domain.DTO.Common;
public partial class OutboundLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId OutboundLogId { get; set; }
    public string? SystemCalledName { get; set; }
    public string? APICalled { get; set; }
    public string? APIMethod { get; set; }
    public string? RequestDetails { get; set; }
    public DateTime RequestDateTime { get; set; }
    public string? ResponseDetails { get; set; }
    public DateTime ResponseDateTime { get; set; }
    public string? ExceptionDetails { get; set; }

    public OutboundLog()
    {
        OutboundLogId = ObjectId.GenerateNewId();
    }
}