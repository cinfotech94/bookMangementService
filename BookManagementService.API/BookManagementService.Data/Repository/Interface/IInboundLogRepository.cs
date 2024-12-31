
using MongoDB.Bson;
using BookManagementService.Domain.DTO.Common;

namespace BookManagementService.Data.Repository.Interface
{
    public interface IInboundLogRepository
    {
        Task<(ObjectId, Exception)> CreateInboundLog(InboundLog inboundLog);
    }
}
