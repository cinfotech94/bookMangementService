using PaymentPurchaseManagementService.Data.context;
using PaymentPurchaseManagementService.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Data.Common;
using PaymentPurchaseManagementService.Data.Context;
using PaymentPurchaseManagementService.Data.Repository.Implementation;
using PaymentPurchaseManagementService.Data.Repository.Interface;

namespace PaymentPurchaseManagementService.Data;
public static class DependecyInjection
{
    public static void AddDataLayerService(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<DappperDbConnection>();
        services.AddScoped<MongoDbContext>();
    //    services.AddDbContext<ApplicationDBContext>(options =>
    //options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("mongoDbLog");
            return new MongoClient(connectionString);
        });
        services.AddScoped<IInboundLogRepository, InboundLogRepository>();
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
    }
}

