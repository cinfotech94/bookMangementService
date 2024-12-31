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
                await GetCachedData();
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
                await GetCachedData();

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
                await GetCachedData();

                var query = @"DELETE FROM carts
                          WHERE username = @Username AND bookId = @BookId";
                using (var connection = _context.CreateConnection())
                {
                    int response = await connection.ExecuteAsync(query, new { Username = username, BookId = bookId });
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
                await GetCachedData();
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
        private async Task GetCachedData()
        {
            string cacheKey = "CartdatabseExist";
            if (!_memoryCache.TryGetValue(cacheKey, out string cachedData))
            {
                // Data not in cache, fetch it
                    try
                    {
                        using (var connection = _context.CreateConnection())
                        {
                            connection.Open();

                            // Step 1: Create Books table if it does not exist
                            var createBooksTableQuery = @"
                                    DO $$
                                    BEGIN
                                        IF NOT EXISTS (
                                            SELECT 1 
                                            FROM information_schema.tables 
                                            WHERE table_name = 'books' AND table_schema = 'public'
                                        ) THEN
                                            CREATE TABLE Books (
                                                Id UUID PRIMARY KEY,
                                                Title VARCHAR(255),
                                                ISBN VARCHAR(255),
                                                Author VARCHAR(255),
                                                PublicationYear VARCHAR(4),
                                                TimeAdded TIMESTAMP,
                                                Genre VARCHAR(255),
                                                Quantity INT DEFAULT 1,
                                                Price FLOAT,
                                                Pages INT,
                                                Description VARCHAR(1000),
                                                Category VARCHAR(255),
                                                NoClick INT DEFAULT 0,
                                                NoOfPurchase INT DEFAULT 0,
                                                NoOfCart INT DEFAULT 0
                                            );
                                        END IF;
                                    END $$;";

                            await connection.ExecuteAsync(createBooksTableQuery);
                            //Console.WriteLine("Books table checked and created if necessary.");

                            // Step 2: Create Carts table if it does not exist
                            var createCartsTableQuery = @"
                                DO $$
                                BEGIN
                                    IF NOT EXISTS (
                                        SELECT 1 
                                        FROM information_schema.tables 
                                        WHERE table_name = 'carts' AND table_schema = 'public'
                                    ) THEN
                                        CREATE TABLE Carts (
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
