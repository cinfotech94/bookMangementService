using BookManagementService.Data.context;
using BookManagementService.Data.Repository.Interface;
using BookManagementService.Domain.DTO.Request;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Rpc.Context.AttributeContext.Types;

namespace BookManagementService.Data.Repository.Implementation
{
    public class BookRepository : IBookRepository
    {
        private readonly DappperDbConnection _context;
        private readonly IMemoryCache _memoryCache;

        public BookRepository(DappperDbConnection context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<(int, Exception)> AddBookAsync(BookDTO book)
        {
            try
            {
                GetCachedData();

                var query = @"INSERT INTO books (id, title, ISBN, author, publicationYear, genre, quantity, price, pages, description, category, noClick, noOfPPurchase, noOfCart)
                      VALUES (@Id, @Title, @ISBN, @Author, @PublicationYear, @Genre, @Quantity, @Price, @Pages, @Description, @Category, @NoClick, @NoOfPPurchase, @NoOfCart)";
                using (var connection = _context.CreateConnection())
                {
                    int response = 0;
                    response = await connection.ExecuteAsync(query, new
                    {
                        Id = Guid.NewGuid(),
                        book.title,
                        book.ISBN,
                        book.author,
                        book.publicationYear,
                        book.genre,
                        book.quantity,
                        book.price,
                        book.pages,
                        book.description,
                        book.category,
                        book.noClick,
                        book.noOfPPurchase,
                        book.noOfCart
                    });
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (0, ex);
            }
        }

        public async Task<(BookDTO, Exception)> GetBookByIdAsync(Guid id)
        {
                var response = new BookDTO();
            try
            {
                GetCachedData();

                var query = "SELECT * FROM books WHERE id = @Id";
            using (var connection = _context.CreateConnection())
            {
                    response= await connection.QuerySingleOrDefaultAsync<BookDTO>(query, new { Id = id });
                    return(response,null);
            }
            }
            catch(Exception ex)
            {
                return (response, ex);
            }

        }

        public async Task<(BookDTO, Exception)> GetBookByISBNAsync(string isbn)
        {
            var response = new BookDTO();
            try
            {
                GetCachedData();

                var query = "SELECT * FROM books WHERE ISBN = @ISBN";
            using (var connection = _context.CreateConnection())
            {
                response= await connection.QuerySingleOrDefaultAsync<BookDTO>(query, new { ISBN = isbn });
                    return(response,null);
            }
            }
            catch( Exception ex)
            {
                return(response, ex);
            }

        }
        public async Task<(List<BookDTO>, int, int, Exception)> GetBooksByAuthorAsync( string author, int pageNumber=1, int pageSize = 10, bool ascending = false, string sortBy = "noClick")
        {
            var response = new List<BookDTO>();
            try
            {
                GetCachedData();

                string order = ascending ? "ASC" : "DESC";
                var query = $@"SELECT * FROM books 
                      WHERE author = @Author 
                      ORDER BY {sortBy} {order} 
                      OFFSET @Offset ROWS 
                      FETCH NEXT @PageSize ROWS ONLY";

                var countQuery = "SELECT COUNT(*) FROM Books WHERE author = @Author";

                using (var connection = _context.CreateConnection())
                {
                    // Get total count
                    var totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new { Author = author });

                    // Get paginated results
                    response = (await connection.QueryAsync<BookDTO>(
                        query,
                        new { Author = author, Offset = (pageNumber - 1) * pageSize, PageSize = pageSize }
                    )).ToList();

                    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                    return (response, totalPages, totalRecords, null);
                }
            }
            catch (Exception ex)
            {
                return (response, 0, 0, ex);
            }
        }

        public async Task<(List<BookDTO>, int, int, Exception)> SearchBooksAsync(string searchText, int pageNumber=1, int pageSize = 10, bool ascending = false, string sortBy = "noClick")
        {
            var response = new List<BookDTO>();
            try
            {
                GetCachedData();

                string order = ascending ? "ASC" : "DESC";
                var query = $@"SELECT * FROM books 
                      WHERE title LIKE @SearchText 
                         OR description LIKE @SearchText 
                         OR category LIKE @SearchText 
                         OR genre LIKE @SearchText
                      ORDER BY {sortBy} {order} 
                      OFFSET @Offset ROWS 
                      FETCH NEXT @PageSize ROWS ONLY";

                var countQuery = @"SELECT COUNT(*) FROM books 
                           WHERE title LIKE @SearchText 
                              OR description LIKE @SearchText 
                              OR category LIKE @SearchText 
                              OR genre LIKE @SearchText";

                using (var connection = _context.CreateConnection())
                {
                    // Get total count
                    var totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new { SearchText = "%" + searchText + "%" });

                    // Get paginated results
                    response = (await connection.QueryAsync<BookDTO>(
                        query,
                        new { SearchText = "%" + searchText + "%", Offset = (pageNumber - 1) * pageSize, PageSize = pageSize }
                    )).ToList();

                    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                    return (response, totalPages, totalRecords, null);
                }
            }
            catch (Exception ex)
            {
                return (response, 0, 0, ex);
            }
        }

        public async Task<(List<BookDTO>, int, int, Exception)> GetAllBooksAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, string sortBy = "noClick")
        {
            var response = new List<BookDTO>();
            try
            {
                GetCachedData();

                string order = ascending ? "ASC" : "DESC";
                var query = $@"SELECT * FROM books 
                       ORDER BY {sortBy} {order} 
                       OFFSET @Offset ROWS 
                       FETCH NEXT @PageSize ROWS ONLY";

                var countQuery = "SELECT COUNT(*) FROM books";

                using (var connection = _context.CreateConnection())
                {
                    // Get total count
                    var totalRecords = await connection.ExecuteScalarAsync<int>(countQuery);

                    // Get paginated results
                    response = (await connection.QueryAsync<BookDTO>(
                        query,
                        new { Offset = (pageNumber - 1) * pageSize, PageSize = pageSize }
                    )).ToList();

                    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                    return (response, totalPages, totalRecords, null);
                }
            }
            catch (Exception ex)
            {
                return (response, 0, 0, ex);
            }
        }


        public async Task<(int, Exception)> UpdateBookAsync(BookDTO book)
        {
            GetCachedData();

            int response = 0;
            try
            {
                var query = @"UPDATE books
                      SET title = @Title, ISBN = @ISBN, author = @Author, publicationYear = @PublicationYear, genre = @Genre,
                          quantity = @Quantity, price = @Price, pages = @Pages, description = @Description, category = @Category,
                          noClick = @NoClick, noOfPPurchase = @NoOfPPurchase, noOfCart = @NoOfCart
                      WHERE ISBN = @ISBN OR id = @Id";
                using (var connection = _context.CreateConnection())
                {
                    response = await connection.ExecuteAsync(query, new
                    {
                        book.title,
                        book.ISBN,
                        book.author,
                        book.publicationYear,
                        book.genre,
                        book.quantity,
                        book.price,
                        book.pages,
                        book.description,
                        book.category,
                        book.noClick,
                        book.noOfPPurchase,
                        book.noOfCart,
                    });
                }
                return (response, null);
            }
            catch (Exception ex)
            {
                return (response, ex);
            }
        }

        public async Task<(int, Exception)> DeleteBookAsync(Guid id)
        {
            GetCachedData();

            int response = 0;
            try { 
            var query = "DELETE FROM books WHERE id = @Id";
            using (var connection = _context.CreateConnection())
            {
                response= await connection.ExecuteAsync(query, new { Id = id });
            }
                return (response, null);
            }
            catch (Exception ex)
            {
                return (response, ex);
            }
        }

        public async Task<(int, Exception)> DeleteBookByISBNAsync(string isbn)
        {
            GetCachedData();

            var response = 0;
            try
            {
            var query = "DELETE FROM books WHERE ISBN = @ISBN";
            using (var connection = _context.CreateConnection())
            {
                response= await connection.ExecuteAsync(query, new { ISBN = isbn });
            }
                return (response, null);
            }
            catch (Exception ex)
            {
                return (response, ex);
            }
        }
        private async Task GetCachedData()
        {
            string cacheKey = "databseExist";
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
                    .SetSlidingExpiration(TimeSpan.FromDays(365)) // Refresh expiration after each access
                    .SetAbsoluteExpiration(TimeSpan.FromDays(500)); // Cache expires after 1 hour

                // Save data in cache
                _memoryCache.Set(cacheKey, cachedData, cacheEntryOptions);
            }
        }
    }

}
