using BookManagementService.Domain.DTO.Common;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Service.RabbitMQServices;
public class RequestPublish
{

    private readonly IBus _bus;

    public RequestPublish(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendRequestMessageAsync(string methodName, object? payload)
    {
        var requestMessage = new RequestMessage
        {
            MethodName = methodName,
            Payload = payload
        };

        // Send the message to the RabbitMQ exchange
        var response = await _bus.Request<RequestMessage, ResponseMessage>(requestMessage);

        // Handle the response
        Console.WriteLine($"Response received: {response.Message.MethodName}");
        Console.WriteLine($"Response payload: {response.Message.Payload}");
    }
}

