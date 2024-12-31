using BookManagementService.Domain.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Data.Repository.Interface
{
    public interface IBookRepository
    {
        Task<(int, Exception)> AddBookAsync(BookDTO book);
        Task<(BookDTO, Exception)> GetBookByIdAsync(Guid id);
        Task<(BookDTO, Exception)> GetBookByISBNAsync(string isbn);
        Task<(List<BookDTO>, int, int, Exception)> GetAllBooksAsync(bool ascending = false, int pageNumber = 1, int pageSize = 10, string sortBy = "noClick");
        Task<(List<BookDTO>, int, int, Exception)> SearchBooksAsync(string searchText, int pageNumber = 1, int pageSize = 10, bool ascending = false, string sortBy = "noClick");
        Task<(List<BookDTO>, int, int, Exception)> GetBooksByAuthorAsync(string author, int pageNumber = 1, int pageSize = 10, bool ascending = false, string sortBy = "noClick");
        Task<(int, Exception)> UpdateBookAsync(BookDTO book);
        Task<(int, Exception)> DeleteBookAsync(Guid id);
        Task<(int, Exception)> DeleteBookByISBNAsync(string isbn);
    }
}
