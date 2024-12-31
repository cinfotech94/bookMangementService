using BookManagementService.Data.Repository.Interface;
using BookManagementService.Domain.DTO.Common;
using BookManagementService.Service.MainServices.Interface;
using MassTransit;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Service.RabbitMQServices;
public class RequestConsumer : IConsumer<RequestMessage>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBookService _bookService;
    private readonly ICartService _cartService;

    public RequestConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _bookService=serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IBookService>();
        _cartService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICartService>();
    }

    public async Task Consume(ConsumeContext<RequestMessage> context)
    {
        var message = context.Message;
        var correlationId = Guid.NewGuid();
        // Depending on the method name, handle the payload
        object? responsePayload = null;
        string caller = message.caller + nameof(Consume);

        switch (message.MethodName)
        {
            case "RemoveBookFromCart":
                responsePayload = await HandleRemoveBookFromCartService(message.Payload, correlationId.ToString(), caller);
                break;
            case "DecreaseBook":
                responsePayload = await HandleDecreaseBookService(message.Payload, correlationId.ToString(), caller);
                break;
            case "ClearCart":
                responsePayload = await HandleClearCartService(message.Payload, correlationId.ToString(), caller);
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

    private async Task<object> HandleRemoveBookFromCartService(object? payload, string caller, string correlationId)
    {
        // Implement logic for BookService
        string request = System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload));
        string[] requests = request.Split("||");
        var response = await _cartService.RemoveBookFromCartAsync(requests[0], requests[1], caller, correlationId);
        return response;
    }
    private async Task<object> HandleDecreaseBookService(object? payload, string caller, string correlationId)
    {
        // Implement logic for BookService
        string request = System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload));
        string[] requests = request.Split("||");
        var response = await _bookService.DecreaseBookQuantityAsync(Guid.Parse(requests[0]),int.Parse( requests[1]), caller, correlationId);
        return response;
    }
    private async Task<object> HandleClearCartService(object? payload, string caller, string correlationId)
    {
        // Implement logic for BookService
        string username = System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload));
        var response = await _cartService.ClearCartAsync(username, caller, correlationId);
        return response;
    }

}
