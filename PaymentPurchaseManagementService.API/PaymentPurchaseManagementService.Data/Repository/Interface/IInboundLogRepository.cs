
using MongoDB.Bson;
using PaymentPurchaseManagementService.Domain.DTO.Common;

namespace PaymentPurchaseManagementService.Data.Repository.Interface
{
    public interface IInboundLogRepository
    {
        Task<(ObjectId, Exception)> CreateInboundLog(InboundLog inboundLog);
    }
}
