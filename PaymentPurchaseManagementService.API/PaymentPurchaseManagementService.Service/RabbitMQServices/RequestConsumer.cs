using PaymentPurchaseManagementService.Domain.DTO.Common;
using MassTransit;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPurchaseManagementService.Service.RabbitMQServices;
public class RequestConsumer : IConsumer<RequestMessage>
{
    private readonly IServiceProvider _serviceProvider;

    public RequestConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<RequestMessage> context)
    {
        var message = context.Message;

        // Depending on the method name, handle the payload
        object? responsePayload = null;

        switch (message.MethodName)
        {

            default:
                throw new Exception($"Unknown method: {message.MethodName}");
        }

        // Create the response message
        var responseMessage = new ResponseMessage
        {
            MethodName = message.MethodName,
            Payload = responsePayload,
            ResponseTime = DateTime.UtcNow
        };

        // Send the response to the appropriate reply queue
        await context.RespondAsync(responseMessage);
    }
}
