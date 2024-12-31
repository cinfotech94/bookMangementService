using BookManagementService.Domain.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Data.Repository.Interface
{
    public interface ICartRepository
    {
        Task<(int, Exception)> AddBookToCartAsync(CartDTO cart);
        Task<(IEnumerable<CartDTO>, Exception)> GetCartByUserAsync(string username);
        Task<(int, Exception)> RemoveBookFromCartAsync(string username, string bookId);
        Task<(int, Exception)> ClearCartAsync(string username);
    }
}
