using PaymentPurchaseManagementService.Domain.DTO.Request;
using PaymentPurchaseManagementService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPurchaseManagementService.Data.Repository.Interface
{
    public interface IPurchaseRepository
    {
        Task<(int, Exception)> AddBookToPurchaseAsync(PurchaseDTO cart);
        Task<(IEnumerable<PurchaseDTO>, Exception)> GetPurchaseByUserAsync(string username);
    }
}
