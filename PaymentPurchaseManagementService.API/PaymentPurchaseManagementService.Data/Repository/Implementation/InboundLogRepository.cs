
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Text.Json;
using PaymentPurchaseManagementService.Data.Context;
using PaymentPurchaseManagementService.Data.Repository.Interface;
using PaymentPurchaseManagementService.Domain.DTO.Common;

namespace PaymentPurchaseManagementService.Data.Repository.Implementation
{
    public class InboundLogRepository : IInboundLogRepository
    {
        private readonly MongoDbContext context;
        private readonly IServiceProvider serviceProvider;
        private readonly IServiceScope scope;

        public InboundLogRepository(IServiceProvider serviceProvider)
        {
            this.scope = serviceProvider.CreateScope();
            serviceProvider = serviceProvider;
            context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
        }

        public async Task<(ObjectId, Exception)> CreateInboundLog(InboundLog inboundLog)
        {
            try
            {
                await context.InboundLogs.InsertOneAsync(inboundLog);
            }
            catch (Exception ex)
            {
                return (ObjectId.Empty,ex);
            }

            return (inboundLog.InboundLogId,null);
        }
    }
}
