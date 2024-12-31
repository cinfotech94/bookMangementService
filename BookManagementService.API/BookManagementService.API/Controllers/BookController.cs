using Asp.Versioning;
using BookManagementService.Domain.DTO.Request;
using BookManagementService.Domain.Entity;
using BookManagementService.Service.MainServices.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementService.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
         _bookService = bookService;
        }
        [HttpPost("AddBookAsync")]
        public async Task<IActionResult> AddBookAsync(BookDTO bookDTO)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.AddBookAsync(bookDTO, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("GetBookByIdAsync")]
        public async Task<IActionResult> GetBookByIdAsync(Guid bookID)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.GetBookByIdAsync(bookID, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("GetBookByISBNAsync")]
        public async Task<IActionResult> GetBookByISBNAsync(string bookIsbn)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.GetBookByISBNAsync(bookIsbn, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("GetAllBooksAsync")]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.GetAllBooksAsync( nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("SearchBooksAsync")]
        public async Task<IActionResult> SearchBooksAsync(string searchText)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.SearchBooksAsync(searchText, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPut("UpdateBookAsync")]
        public async Task<IActionResult> UpdateBookAsync(BookDTO bookDTO)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.UpdateBookAsync(bookDTO, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPut("DeleteBookAsync")]
        public async Task<IActionResult> DeleteBookAsync(Guid bookId)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.DeleteBookAsync(bookId, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpDelete("DeleteBookByISBNAsync")]
        public async Task<IActionResult> DeleteBookByISBNAsync(string isbn)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.DeleteBookByISBNAsync(isbn, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("GetBooksByAuthorAsync")]
        public async Task<IActionResult> GetBooksByAuthorAsync(string author)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.GetBooksByAuthorAsync(author, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("IsBookAvailableAsync")]
        public async Task<IActionResult> IsBookAvailableAsync(Guid bookId)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.IsBookAvailableAsync(bookId, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpPut("DecreaseBookQuantityAsync")]
        public async Task<IActionResult> DecreaseBookQuantityAsync(Guid bookId, int quantityToDecrease )
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.DecreaseBookQuantityAsync(bookId, quantityToDecrease,nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
        [HttpGet("CheckBookPriceAsync")]
        public async Task<IActionResult> CheckBookPriceAsync(Guid bookId)
        {
            Guid correlationId = Guid.NewGuid();
            var response = await _bookService.CheckBookPriceAsync(bookId, nameof(BookController), correlationId.ToString());
            return Ok(response);
        }
    }
}
