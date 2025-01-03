using BookManagementService.Data.context;
using BookManagementService.Data.Repository.Interface;
using BookManagementService.Domain.DTO.Request;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Data.Repository.Implementation
{
    public class CartRepository: ICartRepository
    {
        private readonly DappperDbConnection _context;
        private readonly IMemoryCache _memoryCache;

        public CartRepository(DappperDbConnection dapperConnection, IMemoryCache memoryCache)
        {
            _context = dapperConnection;
            _memoryCache = memoryCache;
        }

        // Add a book to the cart
        public async Task<(int, Exception)> AddBookToCartAsync(CartDTO cart)
        {
            try
            {
                
                var query = @"INSERT INTO carts (username, bookId)
                          VALUES (@Username, @BookId)";
                using (var connection = _context.CreateConnection())
                {
                    int response = await connection.ExecuteAsync(query, new
                    {
                        cart.username,
                        cart.bookId
                    });
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (0, ex);
            }
        }

        // Get all books in a user's cart
        public async Task<(IEnumerable<CartDTO>, Exception)> GetCartByUserAsync(string username)
        {
            try
            {
                

                var query = @"SELECT username, bookId
                          FROM carts
                          WHERE username = @Username";
                using (var connection = _context.CreateConnection())
                {
                    var cartItems = await connection.QueryAsync<CartDTO>(query, new { Username = username });
                    return (cartItems, null);
                }
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        // Remove a book from the cart
        public async Task<(int, Exception)> RemoveBookFromCartAsync(string username, string bookId)
        {
            try
            {
                

                var query = @"DELETE FROM carts
                          WHERE username = @Username AND bookId = @BookId";
                using (var connection = _context.CreateConnection())
                {
                    int response = await connection.ExecuteAsync(query, new { Username = username, BookId =Guid.Parse( bookId )});
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (0, ex);
            }
        }

        // Clear the entire cart for a user
        public async Task<(int, Exception)> ClearCartAsync(string username)
        {
            try
            {
                
                var query = @"DELETE FROM carts WHERE username = @Username";
                using (var connection = _context.CreateConnection())
                {
                    int response = await connection.ExecuteAsync(query, new { Username = username });
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (0, ex);
            }
        }

    }

}
