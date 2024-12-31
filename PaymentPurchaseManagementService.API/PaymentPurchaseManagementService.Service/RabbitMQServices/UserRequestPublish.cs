using PaymentPurchaseManagementService.Domain.DTO.Common;
using MassTransit;
using MassTransit.Clients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPurchaseManagementService.Service.RabbitMQServices;
public class UserRequestPublish
{

    private readonly IRequestClient<RequestMessage> _requestClient;

    public UserRequestPublish(IBus bus)
    {
        // Specify the target queue name for the request
        var serviceAddress = new Uri("exchange:book-service");
        _requestClient = bus.CreateRequestClient<RequestMessage>(serviceAddress);
    }

    public async Task SendRequestMessageAsync(string methodName, object? payload)
    {
        var requestMessage = new RequestMessage
        {
            MethodName = methodName,
            Payload = payload,
            caller = "payment-queue",
        };

        // Send the request and await the response
        var response = await _requestClient.GetResponse<ResponseMessage>(requestMessage);

        // Handle the response
        Console.WriteLine($"Response received: {response.Message.MethodName}");
        Console.WriteLine($"Response payload: {response.Message.Payload}");
    }
}

