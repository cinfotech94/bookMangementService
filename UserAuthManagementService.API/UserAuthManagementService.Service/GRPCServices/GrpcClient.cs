using Grpc.Net.Client;
using UserAuthManagementService.Domain.Protos;

public class GrpcClient
{
    private readonly RequestMessager.RequestMessagerClient _client;

    public GrpcClient(IServiceProvider serviceProvider, string methodName)
    {
        // Dynamically resolve the correct gRPC client based on the methodName
        var channel = GetGrpcChannel(methodName);
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

    private GrpcChannel GetGrpcChannel(string methodName)
    {
        string serverUrl;

        // Switch case to determine the gRPC server URL based on the method name
        switch (methodName)
        {
            case "MethodA":
                serverUrl = "https://server-a.com/GRPC";
                break;
            case "MethodB":
                serverUrl = "https://server-b.com/GRPC";
                break;
            case "MethodC":
                serverUrl = "https://server-c.com/GRPC";
                break;
            default:
                serverUrl = "https://default-server.com/GRPC";
                break;
        }

        // Create and return the GrpcChannel
        var channel = GrpcChannel.ForAddress(serverUrl);
        return channel;
    }
}


////using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Grpc.Net.Client;
//using System.Threading.Tasks;
//using UserAuthManagementService.Domain.Protos;


//namespace UserAuthManagementService.Service.GRPCServices
//{




//    public class GrpcClient
//    {
//        private readonly RequestMessager.RequestMessagerClient _client;

//        public GrpcClient(RequestMessager.RequestMessagerClient client)
//        {
//            _client = client;
//        }

//        public async Task<ResponseMessage> SendRequestMessageAsync(string methodName, string caller, string payload)
//        {
//            // Create the RequestMessage
//            var requestMessage = new RequestMessage
//            {
//                MethodName = methodName,
//                Caller = caller,
//                Payload = payload,
//                RequestTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
//            };

//            // Send the request and get the response
//            var response = await _client.SendRequestAsync(requestMessage);

//            return response;
//        }
//    }


//}
