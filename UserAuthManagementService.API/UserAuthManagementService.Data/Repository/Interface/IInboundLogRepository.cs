
using MongoDB.Bson;
using UserAuthManagementService.Domain.DTO.Common;

namespace UserAuthManagementService.Data.Repository.Interface
{
    public interface IInboundLogRepository
    {
        Task<(ObjectId, Exception)> CreateInboundLog(InboundLog inboundLog);
    }
}
