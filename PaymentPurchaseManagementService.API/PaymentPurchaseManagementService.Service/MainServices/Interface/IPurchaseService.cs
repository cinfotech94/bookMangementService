using PaymentPurchaseManagementService.Domain.DTO.Common;
using PaymentPurchaseManagementService.Domain.DTO.Request;
using PaymentPurchaseManagementService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPurchaseManagementService.Service.MainServices
{
    public interface IPurchaseService
    {
        Task<GenericResponse<int>> AddBookToPurchaseAsync(PurchaseDTO purchase, string caller, string correlationId);
        Task<GenericResponse<IEnumerable<PurchaseDTO>>> GetPurchaseByUserAsync(string username, string caller, string correlationId);
        Task<GenericResponse<IEnumerable<BookDTO>>> PurchaseCart(string username, string caller, string correlationId);
    }
}
