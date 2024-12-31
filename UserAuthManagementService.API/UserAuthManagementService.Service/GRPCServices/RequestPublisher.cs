
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using UserAuthManagementService.Domain.DTO.Request;
using UserAuthManagementService.Domain.Protos;
using UserAuthManagementService.Service.MainServices;
using static Google.Events.Protobuf.Cloud.Batch.V1.JobNotification.Types;

namespace UserAuthManagementService.Service.GRPCServices
{
    public class RequestMessagerService : RequestMessager.RequestMessagerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserServices _userServices;

        public RequestMessagerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userServices = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IUserServices>();

        }
        public async override Task<ResponseMessage> SendRequest(RequestMessage request, ServerCallContext context)
        {
            object? responsePayload = null;
            var correlationId = Guid.NewGuid();
            string caller = request.Caller + nameof(SendRequest);
            switch (request.MethodName)
            {
                case "GetUser":
                    responsePayload = await HandleGetUserService(request.Payload, correlationId.ToString(), caller); 
                    break;
                case "ConfirmToken":
                    responsePayload = await HandleConfirmTokenService(request.Payload, correlationId.ToString(), caller);
                    break;
                case "UpdateBalanace":
                    responsePayload = await HandleUpdateBalanceService(request.Payload, correlationId.ToString(), caller);
                    break;
                case "GenerateToken":
                    responsePayload = await HandleGenerateTokenService(request.Payload, correlationId.ToString(), caller);
                    break;
                default:
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Unknown method"));
            }
            var response = new ResponseMessage
            {
                Caller = request.Caller,
                MethodName = request.MethodName,
                Payload =System.Text.Json.JsonSerializer.Serialize<object>( responsePayload) ?? string.Empty,
                ResponseTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
            };

            return response;
        }
        private async Task<object> HandleGenerateTokenService(object? payload, string caller, string correlationId)
        {
            // Implement logic for BookService
            LoginRequest loginRequest = System.Text.Json.JsonSerializer.Deserialize<LoginRequest>(System.Text.Json.JsonSerializer.Serialize(payload));
            var response = await _userServices.AuthenticateUser(loginRequest, caller, correlationId);
            return response;
        }
        private async Task<object> HandleGetUserService(object? payload, string caller, string correlationId)
        {
            // Implement logic for BookService
            string username = System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload));
            var response = await _userServices.GetUserByUsername(username, caller, correlationId);
            return response;
        }
        private async Task<object> HandleConfirmTokenService(object? payload, string caller, string correlationId)
        {
            // Implement logic for BookService
            ConfirmTokenRequest confirmTokenRequest = System.Text.Json.JsonSerializer.Deserialize<ConfirmTokenRequest>(System.Text.Json.JsonSerializer.Serialize(payload));
            var response = await _userServices.ConfimTokenToken(confirmTokenRequest, caller, correlationId);
            return response;
        }
        private async Task<object> HandleUpdateBalanceService(object? payload, string caller, string correlationId)
        {
            // Implement logic for BookService
            string[] UpdateBalanceRequest = (System.Text.Json.JsonSerializer.Deserialize<string>(System.Text.Json.JsonSerializer.Serialize(payload))).Split("||");
            var response = await _userServices.ChangeBalanceUser(UpdateBalanceRequest[0], double.Parse(UpdateBalanceRequest[1]), caller, correlationId);
            return response;
        }
    }

}
