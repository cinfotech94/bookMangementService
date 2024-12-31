
using PaymentPurchaseManagementService.Domain.Protos;
using Grpc.Core;

namespace PaymentPurchaseManagementService.Service.GRPCServices
{
    public class RequestMessagerService : RequestMessager.RequestMessagerBase
    {
        public async override Task<ResponseMessage> SendRequest(RequestMessage request, ServerCallContext context)
        {
            object? responsePayload = null;

            switch (request.MethodName)
            {
                case "CheckBookPrice":
                    responsePayload = await HandleGetUserService(request);
                    break;
                case "IsBookAvailable":
                    responsePayload = await HandleGetUserService(request);
                    break;
                case "GetBookById":
                    responsePayload = await HandleGetUserService(request);
                    break;
                case "GetBooksInCart":
                    responsePayload = await HandleGetUserService(request);
                    break;
                case "UpdateUser":
                    responsePayload = await HandleGetUserService(request);
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
        private async Task<string> HandleGetUserService(RequestMessage? request)
        {
            // Implement logic for PaymentService
            return "i love you" ?? string.Empty;
        }
    }

}