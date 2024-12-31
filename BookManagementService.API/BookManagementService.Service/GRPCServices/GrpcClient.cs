using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using System.Threading.Tasks;
using BookManagementService.Domain.Protos;


namespace BookManagementService.Service.GRPCServices
{

    public class GrpcClient
    {
        private readonly RequestMessager.RequestMessagerClient _client;

        public GrpcClient(string serverAddress)
        {
            // Create a gRPC channel pointing to the specified server address
            var channel = GrpcChannel.ForAddress(serverAddress);

            // Initialize the gRPC client with the channel
            _client = new RequestMessager.RequestMessagerClient(channel);
        }

        public async Task<ResponseMessage> SendRequestMessageAsync(string methodName, string caller, string payload)
        {
            // Create the RequestMessage
            var requestMessage = new RequestMessage
            {
                MethodName = methodName,
                Caller = caller,
                Payload = payload,
                RequestTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
            };

            // Send the request and get the response
            var response = await _client.SendRequestAsync(requestMessage);

            return response;
        }
    }


}
