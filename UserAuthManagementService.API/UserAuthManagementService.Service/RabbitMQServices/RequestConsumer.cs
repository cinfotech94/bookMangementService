using UserAuthManagementService.Domain.DTO.Common;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthManagementService.Domain.DTO.Request;
using UserAuthManagementService.Service.MainServices;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace UserAuthManagementService.Service.RabbitMQServices;
public class RequestConsumer : IConsumer<RequestMessage>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserServices _userServices;

    public RequestConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _userServices = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IUserServices>();
    }

    public async Task Consume(ConsumeContext<RequestMessage> context)
    {
        var message = context.Message;
        var correlationId=Guid.NewGuid();
        // Depending on the method name, handle the payload
        object? responsePayload = null;
        string caller=message.caller+nameof(Consume);
        switch (message.MethodName)
        {
            case "Audit":
                responsePayload = await HandleAuditService(message.Payload, correlationId.ToString(),caller);
                break;
            default:
                throw new Exception($"Unknown method: {message.MethodName}");
        }

        // Create the response message
        var responseMessage = new ResponseMessage
        {
            MethodName = message.MethodName,
            Payload = responsePayload,
            ResponseTime = DateTime.UtcNow,
            caller= message.caller,
        };

        // Send the response to the appropriate reply queue
        await context.RespondAsync(responseMessage);
    }

   private async Task<object?> HandleAuditService(object? payload, string caller,string correlationId)
    {
        // Implement logic for BookService
        AuditDTO auditDTO = System.Text.Json.JsonSerializer.Deserialize<AuditDTO>(System.Text.Json.JsonSerializer.Serialize(payload));
        var response= await _userServices.AuditUser(auditDTO, caller, correlationId);
        return response;
    }
}
