using Asp.Versioning;
using BookManagementService.Domain.DTO.Request;
using BookManagementService.Domain.Entity;
using BookManagementService.Service.MainServices.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementService.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost("AddBookToPurchaseAsync")]
        public async Task<IActionResult> AddBookToCartAsync(CartDTO cartDTO)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _cartService.AddBookToCartAsync(cartDTO, nameof(CartController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("GetCartByUserAsync")]
        public async Task<IActionResult> GetCartByUserAsync(string username)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _cartService.GetCartByUserAsync(username, nameof(CartController), correlationId.ToString());
            return Ok(response);
        }
        [HttpDelete("RemoveBookFromCartAsync")]
        public async Task<IActionResult> RemoveBookFromCartAsync(string username, string bookID)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _cartService.RemoveBookFromCartAsync(username, bookID, nameof(CartController), correlationId.ToString());
            return Ok(response);
        }
        [HttpDelete("ClearCartAsync")]
        public async Task<IActionResult> ClearCartAsync(string username)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _cartService.ClearCartAsync(username, nameof(CartController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("GetBooksInCart")]
        public async Task<IActionResult> GetBooksInCart(string username)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _cartService.GetBooksInCart(username, nameof(CartController), correlationId.ToString());
            return Ok(response);
        }
    }
}
