using BookManagementService.Domain.DTO.Common;
using BookManagementService.Domain.DTO.Request;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Service.MainServices.Interface
{
    public interface IBookService
    {
        Task<GenericResponse<BookDTO>> GetBookByIdAsync(Guid id, string caller, string correlationId);

        Task<GenericResponse<BookDTO>> GetBookByISBNAsync(string isbn, string caller, string correlationId);

        Task<GenericResponse<List<BookDTO>>> GetAllBooksAsync( string caller, string correlationId,bool ascending = false, int pageNumber = 1, int pageSize = 10, string sortBy = "noClick");

        Task<GenericResponse<List<BookDTO>>> SearchBooksAsync(string searchText, string caller, string correlationId, int pageNumber = 1, int pageSize = 10, bool ascending = false, string sortBy = "noClick");

        Task<GenericResponse<List<BookDTO>>> GetBooksByAuthorAsync(string author, string caller, string correlationId, int pageNumber = 1, int pageSize = 10, bool ascending = false, string sortBy = "noClick");

        Task<GenericResponse<int>> AddBookAsync(BookDTO book, string caller, string correlationId);

        Task<GenericResponse<int>> UpdateBookAsync(BookDTO book, string caller, string correlationId);

        Task<GenericResponse<int>> DeleteBookAsync(Guid id, string caller, string correlationId);

        Task<GenericResponse<int>> DeleteBookByISBNAsync(string isbn, string caller, string correlationId);
        Task<GenericResponse<bool>> IsBookAvailableAsync(Guid id, string caller, string correlationId);
        Task<GenericResponse<int>> DecreaseBookQuantityAsync(Guid id, int quantityToDecrease, string caller, string correlationId);
        Task<GenericResponse<double>> CheckBookPriceAsync(Guid id, string caller, string correlationId);


    }
}
