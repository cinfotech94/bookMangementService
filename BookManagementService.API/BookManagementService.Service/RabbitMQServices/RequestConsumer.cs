using BookManagementService.Domain.DTO.Common;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Service.RabbitMQServices;
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
            case "ProcessBook":
                responsePayload = await HandleBookService(message.Payload);
                break;
            case "ProcessPayment":
                responsePayload = await HandlePaymentService(message.Payload);
                break;
            case "ProcessUser":
                responsePayload = await HandleUserService(message.Payload);
                break;
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

    private Task<object?> HandleBookService(object? payload)
    {
        // Implement logic for BookService
        return Task.FromResult<object?>(null);
    }

    private Task<object?> HandlePaymentService(object? payload)
    {
        // Implement logic for PaymentService
        return Task.FromResult<object?>(null);
    }

    private Task<object?> HandleUserService(object? payload)
    {
        // Implement logic for UserService
        return Task.FromResult<object?>(null);
    }
}
