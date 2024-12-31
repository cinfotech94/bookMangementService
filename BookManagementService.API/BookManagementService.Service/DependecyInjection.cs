using BookManagementService.Service.RabbitMQServices;
using BookManagementService.Service.GenericServices.Implemetation;
using BookManagementService.Service.GenericServices.Interface;
using BookManagementService.Service.GenericServices.Static;
using BookManagementService.Data.context;
using BookManagementService.Data.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Data.Common;
using BookManagementService.Data.Repository.Implementation;
using BookManagementService.Data.Repository.Interface;
using BookManagementService.ServiceGeneric.Services.Interface;
//using BookManagementService.Domain.Protos;
//using BookManagementService.Service.BackGoundService;
using RabbitMQ.Client;
using BookManagementService.Domain.Protos;
//using BookManagementService.Service.MainServices;
//using BookManagementService.Domain.Protos;
//using BookManagementService.Service.GRPCServices;

namespace BookManagementService.service;
public static class DependecyInjection
{
    public static void AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IConnection>(sp =>
        //{
        //    var factory = new ConnectionFactory() { HostName = "localhost", UserName = "CivisAccountQueryHub", Password = "CivisAccountQueryHub123" };
        //    return factory.CreateConnection();

        //});
        //services.AddSingleton<IModel>(sp =>
        //{
        //    var connection = sp.GetRequiredService<IConnection>();
        //    return connection.CreateModel();
        //});
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
                cfg.ReceiveEndpoint("payment-service", e =>
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
        });

        services.AddMassTransitHostedService();
        services.AddScoped<MailerService>();
        services.AddScoped<ElasticsearchService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IIpService, IpService>();
        services.AddScoped<ILoggingService, LoggingService>();
        services.AddScoped<IRestHelper, RestHelper>();
        //services.AddSingleton<JobService>();
        //services.AddHostedService<JobWorker>();
    }
}

