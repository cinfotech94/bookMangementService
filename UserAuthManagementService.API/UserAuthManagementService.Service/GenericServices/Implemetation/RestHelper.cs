using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using MongoDB.Bson;
using UserAuthManagementService.Domain.Enums;
using UserAuthManagementService.Domain.DTO.Common;
using UserAuthManagementService.Service.GenericServices.Interface;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UserAuthManagementService.Service.GenericServices.Interface;
    public class RestHelper : IRestHelper
    {
        public async Task<Result<TResponse>> ConsumeApi<TResponse>(string url, object? payload, string serviceProvider,string caller, string corelationId,
            ApiType type = ApiType.Get,
            Dictionary<string, string> headers = null, bool logRequest = true, bool logResponse = true)
        {
        caller= caller +nameof(ConsumeApi);
            var apiResult = new Result<TResponse>();
            var outboundLog = new OutboundLog
            {
                APICalled = url,
                APIMethod = serviceProvider,
                ResponseDateTime = DateTime.UtcNow.AddHours(1),
                RequestDateTime = DateTime.UtcNow.AddHours(1),
                SystemCalledName = "UserAuthManagementService"
            };

            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    CheckCertificateRevocationList = false
                };

                using (var client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(200);
                    var requestMessage = new HttpRequestMessage
                    {
                        RequestUri = new Uri(url),
                        Method = type switch
                        {
                            ApiType.Post => HttpMethod.Post,
                            ApiType.Put => HttpMethod.Put,
                            ApiType.Patch => HttpMethod.Patch,
                            ApiType.Delete => HttpMethod.Delete,
                            _ => HttpMethod.Get
                        }
                    };

                    if (headers != null)
                    {
                        foreach (var item in headers)
                        {
                            requestMessage.Headers.Add(item.Key, item.Value);
                        }
                    }

                    if (payload != null)
                    {
                        // Create the content and set the Content-Type header
                        var serializedPayload = JsonSerializer.Serialize(payload);
                        var content = new StringContent(serializedPayload, System.Text.Encoding.UTF8, "application/json");
                        requestMessage.Content = content;
                    }

                    HttpResponseMessage response;
                    if (type == ApiType.Get && headers == null)
                    {
                        response = await client.GetAsync(url);
                    }
                    else
                    {
                        response = await client.SendAsync(requestMessage);
                        if (type == ApiType.Post && response.StatusCode == HttpStatusCode.MethodNotAllowed)
                        {
                            response = await client.PutAsync(url, requestMessage.Content);
                        }
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            apiResult.Content = System.Text.Json.JsonSerializer.Deserialize<TResponse>(result);
                        }
                        catch (JsonException)
                        {
                            // Handle deserialization error
                        }

                        apiResult.Message = "Successful";
                        apiResult.IsSuccess = true;
                        return apiResult;
                    }

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        apiResult.Message = "Not Found";
                        apiResult.IsSuccess = false;
                        return apiResult;
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        apiResult.Content = System.Text.Json.JsonSerializer.Deserialize<TResponse>(result);
                        apiResult.Message = "Bad Request";
                        apiResult.IsSuccess = false;
                        return apiResult;
                    }

                    apiResult.Content = System.Text.Json.JsonSerializer.Deserialize<TResponse > (result);
                    apiResult.IsSuccess = false;
                    return apiResult;
                }
            }
            catch (Exception ex)
            {
                apiResult.Content = (TResponse)(object)null;
                apiResult.IsSuccess = false;
                return apiResult;
            }
        }
    }
