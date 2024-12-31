using BookManagementService.Domain.DTO.Common;
using BookManagementService.Domain.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Service.MainServices.Interface
{
    public interface ICartService
    {
        Task<GenericResponse<int>> AddBookToCartAsync(CartDTO cart, string caller, string correlationId);
        Task<GenericResponse<IEnumerable<CartDTO>>> GetCartByUserAsync(string username, string caller, string correlationId);
        Task<GenericResponse<int>> RemoveBookFromCartAsync(string username, string bookId, string caller, string correlationId);
        Task<GenericResponse<int>> ClearCartAsync(string username, string caller, string correlationId);
        Task<GenericResponse<IEnumerable<BookDTO>>> GetBooksInCart(string username, string caller, string correlationId);
    }
}
