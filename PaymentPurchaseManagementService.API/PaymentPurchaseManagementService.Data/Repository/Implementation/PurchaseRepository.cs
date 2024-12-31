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
                GetCachedData();
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
                GetCachedData();

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
        private async Task GetCachedData()
        {
            string cacheKey = "purchasedatabseExist";
            if (!_memoryCache.TryGetValue(cacheKey, out string cachedData))
            {
                // Data not in cache, fetch it
                    try
                    {
                        using (var connection = _context.CreateConnection())
                        {
                            connection.Open();

                            // Step 2: Create Carts table if it does not exist
                            var createCartsTableQuery = @"
                                DO $$
                                BEGIN
                                    IF NOT EXISTS (
                                        SELECT 1 
                                        FROM information_schema.tables 
                                        WHERE table_name = 'purchases' AND table_schema = 'public'
                                    ) THEN
                                        CREATE TABLE purchases (
                                            Username VARCHAR(255),
                                            BookId UUID,
                                            PRIMARY KEY (Username, BookId)
                                        );
                                    END IF;
                                END $$;";

                            await connection.ExecuteAsync(createCartsTableQuery);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
                    }
                cachedData = "yes";

                // Set cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Refresh expiration after each access
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Cache expires after 1 hour

                // Save data in cache
                _memoryCache.Set(cacheKey, cachedData, cacheEntryOptions);
            }
        }

    }

}
