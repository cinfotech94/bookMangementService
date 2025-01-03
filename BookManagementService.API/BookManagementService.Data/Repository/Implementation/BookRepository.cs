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
                

                var query = @"INSERT INTO books (id, title, ISBN, author, publicationYear, genre, quantity, price, pages, description, category, noClick, noOfPurchase, noOfCart)
                      VALUES (@Id, @Title, @ISBN, @Author, @PublicationYear, @Genre, @Quantity, @Price, @Pages, @Description, @Category, @NoClick, @noOfPurchase, @NoOfCart)";
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
                        book.noOfPurchase,
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
                

                var query = "SELECT * FROM books WHERE id = @Id";
                using (var connection = _context.CreateConnection())
                {
                    response = await connection.QuerySingleOrDefaultAsync<BookDTO>(query, new { Id = id });
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (response, ex);
            }

        }

        public async Task<(BookDTO, Exception)> GetBookByISBNAsync(string isbn)
        {
            var response = new BookDTO();
            try
            {
                

                var query = "SELECT * FROM books WHERE ISBN = @ISBN";
                using (var connection = _context.CreateConnection())
                {
                    response = await connection.QuerySingleOrDefaultAsync<BookDTO>(query, new { ISBN = isbn });
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (response, ex);
            }

        }
        public async Task<(List<BookDTO>, int, int, Exception)> GetBooksByAuthorAsync(string author, int pageNumber = 1, int pageSize = 10, bool ascending = false, string sortBy = "noClick")
        {
            var response = new List<BookDTO>();
            try
            {
                

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

        public async Task<(List<BookDTO>, int, int, Exception)> SearchBooksAsync(string searchText, int pageNumber = 1, int pageSize = 10, bool ascending = false, string sortBy = "noClick")
        {
            var response = new List<BookDTO>();
            try
            {
                

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
            

            int response = 0;
            try
            {
                var query = @"UPDATE books
                      SET title = @Title, ISBN = @ISBN, author = @Author, publicationYear = @PublicationYear, genre = @Genre,
                          quantity = @Quantity, price = @Price, pages = @Pages, description = @Description, category = @Category,
                          noClick = @NoClick, noOfPurchase = @NoOfPurchase, noOfCart = @NoOfCart
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
                        book.noOfPurchase,
                        book.noOfCart,
                        Id=book.id
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
            

            int response = 0;
            try
            {
                var query = "DELETE FROM books WHERE id = @Id";
                using (var connection = _context.CreateConnection())
                {
                    response = await connection.ExecuteAsync(query, new { Id = id });
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
            

            var response = 0;
            try
            {
                var query = "DELETE FROM books WHERE ISBN = @ISBN";
                using (var connection = _context.CreateConnection())
                {
                    response = await connection.ExecuteAsync(query, new { ISBN = isbn });
                }
                return (response, null);
            }
            catch (Exception ex)
            {
                return (response, ex);
            }
        }
    }
}


