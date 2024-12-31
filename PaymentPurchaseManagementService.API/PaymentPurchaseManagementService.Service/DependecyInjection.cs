using PaymentPurchaseManagementService.Service.RabbitMQServices;
using PaymentPurchaseManagementService.Service.GenericServices.Implemetation;
using PaymentPurchaseManagementService.Service.GenericServices.Interface;
using PaymentPurchaseManagementService.Service.GenericServices.Static;
using PaymentPurchaseManagementService.Data.context;
using PaymentPurchaseManagementService.Data.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Data.Common;
using PaymentPurchaseManagementService.Data.Repository.Implementation;
using PaymentPurchaseManagementService.Data.Repository.Interface;
using PaymentPurchaseManagementService.ServiceGeneric.Services.Interface;
//using PaymentPurchaseManagementService.Domain.Protos;
//using PaymentPurchaseManagementService.Service.BackGoundService;
using RabbitMQ.Client;
using PaymentPurchaseManagementService.Domain.Protos;
using PaymentPurchaseManagementService.Service.MainServices;
using PaymentPurchaseManagementService.Service.MainServices.Implementation;
using PaymentPurchaseManagementService.Service.GRPCServices;
//using PaymentPurchaseManagementService.Service.MainServices;
//using PaymentPurchaseManagementService.Domain.Protos;
//using PaymentPurchaseManagementService.Service.GRPCServices;

namespace PaymentPurchaseManagementService.service;
public static class DependecyInjection
{
    public static void AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpc();
        services.AddMassTransit(x =>
        {
            x.AddConsumer<RequestConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqUsername = configuration["RabbitMqQ:Username"];
                var rabbitMqPassword = configuration["RabbitMqQ:Password"];

                try
                {
                    // Connect to RabbitMQ running on localhost and exposed port 5672
                    cfg.Host("localhost", 5672, "/", h =>
                    {
                        h.Username(rabbitMqUsername);
                        h.Password(rabbitMqPassword);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error configuring RabbitMQ: {ex.Message}");
                    throw;
                }

                // Define receive endpoints
                cfg.ReceiveEndpoint("book-service", e =>
                {
                    e.ConfigureConsumer<RequestConsumer>(context);
                });
                cfg.ReceiveEndpoint("user-service", e =>
                {
                    e.ConfigureConsumer<RequestConsumer>(context);
                });
            });
        });

        services.AddGrpcClient<RequestMessager.RequestMessagerClient>(o =>
        {
            o.Address = new Uri("https://localhost:7041/GRPC");  // Point to your gRPC server's URL
            o.Address = new Uri("https://localhost:7018/GRPC");  // Point to your gRPC server's URL
        });

        services.AddMassTransitHostedService();
        services.AddScoped<MailerService>();
        services.AddScoped<MyGrpcClient>();
        services.AddScoped<BookRequestPublish>();
        services.AddScoped<UserRequestPublish>();
        services.AddScoped<ElasticsearchService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        services.AddScoped<IIpService, IpService>();
        services.AddScoped<ILoggingService, LoggingService>();
        services.AddScoped<IRestHelper, RestHelper>();
        //services.AddSingleton<JobService>();
        //services.AddHostedService<JobWorker>();
    }
}

