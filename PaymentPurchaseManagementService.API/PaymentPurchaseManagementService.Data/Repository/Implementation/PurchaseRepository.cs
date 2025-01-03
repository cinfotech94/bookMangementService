using PaymentPurchaseManagementService.Data.context;
using PaymentPurchaseManagementService.Data.Repository.Interface;
using PaymentPurchaseManagementService.Domain.DTO.Request;
using PaymentPurchaseManagementService.Domain.Entity;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPurchaseManagementService.Data.Repository.Implementation
{
    public class PurchaseRepository: IPurchaseRepository
    {
        private readonly DappperDbConnection _context;
        private readonly IMemoryCache _memoryCache;

        public PurchaseRepository(DappperDbConnection dapperConnection, IMemoryCache memoryCache)
        {
            _context = dapperConnection;
            _memoryCache = memoryCache;
        }

        // Add a book to the cart
        public async Task<(int, Exception)> AddBookToPurchaseAsync(PurchaseDTO cart)
        {
            try
            {
                
                var query = @"INSERT INTO purchases (username, bookId)
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
        public async Task<(IEnumerable<PurchaseDTO>, Exception)> GetPurchaseByUserAsync(string username)
        {
            try
            {
                

                var query = @"SELECT username, bookId
                          FROM purchases
                          WHERE username = @Username";
                using (var connection = _context.CreateConnection())
                {
                    var cartItems = await connection.QueryAsync<PurchaseDTO>(query, new { Username = username });
                    return (cartItems, null);
                }
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }
    }

}
