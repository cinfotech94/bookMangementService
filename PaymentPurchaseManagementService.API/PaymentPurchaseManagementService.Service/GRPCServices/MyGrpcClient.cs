using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using System.Threading.Tasks;
using PaymentPurchaseManagementService.Domain.Protos;


namespace PaymentPurchaseManagementService.Service.GRPCServices
{




    public class MyGrpcClient
    {
        private readonly RequestMessager.RequestMessagerClient _client;
        private readonly string bookGrpcUrl = "localHost:23422/grpc";
        private readonly string userGrpcUrl = "localHost:23422/grpc";
        private readonly RequestMessager.RequestMessagerClient _bookClient;
        private readonly RequestMessager.RequestMessagerClient _userClient;
        public MyGrpcClient(RequestMessager.RequestMessagerClient client)
        {
            _client = client;
            var bookChannel = GrpcChannel.ForAddress(bookGrpcUrl);
            var userChannel = GrpcChannel.ForAddress(userGrpcUrl);
            _bookClient = new RequestMessager.RequestMessagerClient(bookChannel);
            _userClient = new RequestMessager.RequestMessagerClient(userChannel);
        }

        public async Task<ResponseMessage> SendBookRequestBookMessageAsync(string methodName, string caller, string payload)
        {
            //Create the RequestMessage
           var requestMessage = new RequestMessage
           {
               MethodName = methodName,
               Caller = caller,
               Payload = payload,
               RequestTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
           };

            //Send the request and get the response
           var response = await _bookClient.SendRequestAsync(requestMessage);

            return response;
        }
        public async Task<ResponseMessage> SendBookRequestUserMessageAsync(string methodName, string caller, string payload)
        {
            //Create the RequestMessage
           var requestMessage = new RequestMessage
           {
               MethodName = methodName,
               Caller = caller,
               Payload = payload,
               RequestTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
           };

            //Send the request and get the response
           var response = await _userClient.SendRequestAsync(requestMessage);

            return response;
        }
    }


}
