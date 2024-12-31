using UserAuthManagementService.Service.RabbitMQServices;
using UserAuthManagementService.Service.GenericServices.Implemetation;
using UserAuthManagementService.Service.GenericServices.Interface;
using UserAuthManagementService.Service.GenericServices.Static;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserAuthManagementService.ServiceGeneric.Services.Interface;
using UserAuthManagementService.Service.BackGoundService;
using UserAuthManagementService.Domain.Protos;
using UserAuthManagementService.Service.MainServices;
using UserAuthManagementService.Service.JWtService;

namespace UserAuthManagementService.Service;
public static class DependecyInjection
{
    public static void AddServiceLayer(this IServiceCollection services,IConfiguration configuration)
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
                cfg.ReceiveEndpoint("payment-service", e =>
                {
                    e.ConfigureConsumer<RequestConsumer>(context);
                });
            });
        });
        services.AddGrpcClient<RequestMessager.RequestMessagerClient>(o =>
        {
            o.Address = new Uri("https://localhost:7041/GRPC");  // Point to your gRPC server's URL
            o.Address = new Uri("https://localhost:7042/GRPC");  // Point to your gRPC server's URL
        });

        services.AddMassTransitHostedService();
        services.AddScoped<MailerService>();
        services.AddScoped<ElasticsearchService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IIpService, IpService>();
        services.AddScoped<ILoggingService,LoggingService>();
        services.AddScoped<IRestHelper,RestHelper>();
        services.AddScoped<AuthService>();
        services.AddScoped<JwtTokenService>();
        services.AddSingleton<JobService>();
        services.AddHostedService<JobWorker>();
        services.AddScoped<IUserServices, UserServices>();

    }
}

