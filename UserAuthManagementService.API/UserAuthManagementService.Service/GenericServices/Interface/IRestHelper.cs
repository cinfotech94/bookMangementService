using UserAuthManagementService.Domain.Enums;
using UserAuthManagementService.Domain.DTO.Common;

namespace UserAuthManagementService.Service.GenericServices.Interface;

public interface IRestHelper
{
    Task<Result<TResponse>> ConsumeApi<TResponse>(string url, object? payload, string serviceProvider, string caller, string corelationId,
            ApiType type = ApiType.Get,
            Dictionary<string, string> headers = null, bool logRequest = true, bool logResponse = true);
}