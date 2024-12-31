using BookManagementService.Data.context;
using BookManagementService.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Data.Common;
using BookManagementService.Data.Context;
using BookManagementService.Data.Repository.Implementation;
using BookManagementService.Data.Repository.Interface;

namespace BookManagementService.Data;
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
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
    }
}

