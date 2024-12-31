using PaymentPurchaseManagementService.Domain.Enums;
using PaymentPurchaseManagementService.Domain.DTO.Common;

namespace PaymentPurchaseManagementService.Service.GenericServices.Interface;

public interface IRestHelper
{
    Task<Result<TResponse>> ConsumeApi<TResponse>(string url, object? payload, string serviceProvider, ApiType type = ApiType.Get, Dictionary<string, string> headers = null, bool logRequest = true, bool logResponse = true);
}