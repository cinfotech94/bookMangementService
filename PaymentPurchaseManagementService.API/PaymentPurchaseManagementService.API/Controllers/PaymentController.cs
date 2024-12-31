using Asp.Versioning;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentPurchaseManagementService.Domain.Entity;
using PaymentPurchaseManagementService.Service.MainServices;
using PaymentPurchaseManagementService.Service.MainServices.Implementation;

namespace PaymentPurchaseManagementService.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        public PaymentController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }
        [HttpPost("AddBookToPurchaseAsync")]
        public async Task<IActionResult> AddBookToPurchaseAsync(PurchaseDTO purchaseDTO)
        {
            Guid correlationId= Guid.NewGuid();
            var response=await _purchaseService.AddBookToPurchaseAsync(purchaseDTO, nameof(PaymentController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("GetPurchaseByUserAsync")]
        public async Task<IActionResult> GetPurchaseByUserAsync(string username)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _purchaseService.GetPurchaseByUserAsync(username, nameof(PaymentController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPut("PurchaseCart")]
        public async Task<IActionResult> PurchaseCart(string username)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _purchaseService.PurchaseCart(username, nameof(PaymentController), correlationId.ToString());
            return Ok(response);
        }
    }
}
