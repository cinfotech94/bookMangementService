
using BookManagementService.Domain.Protos;
using BookManagementService.Service.MainServices.Interface;
using Grpc.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BookManagementService.Service.GRPCServices
{
    public class RequestMessagerService : RequestMessager.RequestMessagerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBookService _bookService;
        private readonly ICartService _cartService;
        public RequestMessagerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _bookService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IBookService>();
            _cartService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICartService>();
        }
        public async override Task<ResponseMessage> SendRequest(RequestMessage request, ServerCallContext context)
        {
            object? responsePayload = null;
            var correlationId = Guid.NewGuid();
            string caller = request.Caller + nameof(SendRequest);
            switch (request.MethodName)
            {
                case "CheckBookPrice":
                    responsePayload = await HandleCheckBookPriceService(request, correlationId.ToString(), caller);
                    break;
                case "IsBookAvailable":
                    responsePayload = await HandleIsBookAvailablerService(request, correlationId.ToString(), caller);
                    break;
                case "GetBookById":
                    responsePayload = await HandleGetBookByIdService(request, correlationId.ToString(), caller);
                    break;
                case "GetBooksInCart":
                    responsePayload = await HandleGetBooksInCartService(request, correlationId.ToString(), caller);
                    break;
                case "RemoveBookFromCartAsync":
                    responsePayload = await HandleRemoveBookFromCartService(request, correlationId.ToString(), caller);
                    break;
                default:
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Unknown method"));
            }
            var response = new ResponseMessage
            {
                Caller = request.Caller,
                MethodName = request.MethodName,
                Payload = System.Text.Json.JsonSerializer.Serialize<object>(responsePayload) ?? string.Empty,
                ResponseTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
            };

            return response;
        }
        private async Task<object> HandleCheckBookPriceService(object? payload, string caller, string correlationId)
        {
            // Implement logic for BookService
            string bookId = System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload));
            var response = await _bookService.CheckBookPriceAsync(Guid.Parse(bookId), caller, correlationId);
            return response;
        }
        private async Task<object> HandleIsBookAvailablerService(object? payload, string caller, string correlationId)
        {
            // Implement logic for BookService
            string bookId = System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload));
            var response = await _bookService.IsBookAvailableAsync(Guid.Parse(bookId), caller, correlationId);
            return response;
        }
        private async Task<object> HandleGetBookByIdService(object? payload, string caller, string correlationId)
        {
            // Implement logic for BookService
            string bookId = System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload));
            var response = await _bookService.GetBookByIdAsync(Guid.Parse(bookId), caller, correlationId);
            return response;
        }
        private async Task<object> HandleGetBooksInCartService(object? payload, string caller, string correlationId)
        {
            // Implement logic for BookService
            string username = System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload));
            var response = await _cartService.GetBooksInCart(username, caller, correlationId);
            return response;
        }
        private async Task<object> HandleRemoveBookFromCartService(object? payload, string caller, string correlationId)
        {
            // Implement logic for BookService
            string request = System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload));
            string[] requests = request.Split("||");
            var response = await _cartService.RemoveBookFromCartAsync(requests[0], requests[1], caller, correlationId);
            return response;
        }
    }

}