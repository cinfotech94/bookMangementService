using BookManagementService.Data.Repository.Interface;
using BookManagementService.Domain.DTO.Common;
using BookManagementService.Domain.DTO.Request;
using BookManagementService.Domain.Entity;
using BookManagementService.Service.GenericServices.Interface;
using BookManagementService.Service.MainServices.Interface;
using BookManagementService.Service.RabbitMQServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Service.MainServices.Implementation
{
    public class CartService:ICartService
    {
        private readonly ILoggingService _loggingService;
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;

        public CartService(RequestPublish serviceClient, ILoggingService loggingService, ICartRepository cartRepository, IBookRepository bookRepository)
        {
            _loggingService = loggingService;
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
        }
        public async Task<GenericResponse<int>> AddBookToCartAsync(CartDTO cart, string caller, string correlationId)
        {
            caller += "||" + nameof(AddBookToCartAsync);
            GenericResponse<int> response = new GenericResponse<int>();

            try
            {
                var (book, bookRxception) = await _bookRepository.GetBookByIdAsync(cart.bookId);
                if (book==null)
                {
                    response.message = "book not exist";
                    response.status = false;
                    response.data = 0;
                }
                var books=await GetCartByUserAsync(cart.username, caller, correlationId);
                if(books.data.Any(u=>u.bookId==cart.bookId))
                {
                    response.message = "existing cart already";
                    response.status = false;
                    response.data = 0;
                }
                var (result, exception) = await _cartRepository.AddBookToCartAsync(cart);
                if (exception != null)
                {
                    await _loggingService.LogError($"Failed to add book to cart for user {cart.username}", caller, exception, correlationId);
                    response.message = "Failed to add book to cart.";
                    response.status = false;
                    response.data = 0;
                }
                else
                {
                    await _loggingService.LogInformation($"Book added to cart successfully for user {cart.username}", caller, correlationId);
                    response.message = "Book added to cart successfully.";
                    response.status = true;
                    response.data = result;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"An error occurred while adding book to cart for user {cart.username}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = 0;
            }

            return response;
        }
        public async Task<GenericResponse<IEnumerable<CartDTO>>> GetCartByUserAsync(string username, string caller, string correlationId)
        {
            caller += "||" + nameof(GetCartByUserAsync);
            GenericResponse<IEnumerable<CartDTO>> response = new GenericResponse<IEnumerable<CartDTO>>();

            try
            {
                var (cartItems, exception) = await _cartRepository.GetCartByUserAsync(username);
                if (exception != null)
                {
                    await _loggingService.LogError($"Failed to retrieve cart for user {username}", caller, exception, correlationId);
                    response.message = "Failed to retrieve cart.";
                    response.status = false;
                    response.data = null;
                }
                else
                {
                    await _loggingService.LogInformation($"Cart retrieved successfully for user {username}", caller, correlationId);
                    response.message = "Cart retrieved successfully.";
                    response.status = true;
                    response.data = cartItems;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"An error occurred while retrieving cart for user {username}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = null;
            }

            return response;
        }
        public async Task<GenericResponse<int>> RemoveBookFromCartAsync(string username, string bookId, string caller, string correlationId)
        {
            caller += "||" + nameof(RemoveBookFromCartAsync);
            GenericResponse<int> response = new GenericResponse<int>();

            try
            {
                var (result, exception) = await _cartRepository.RemoveBookFromCartAsync(username, bookId);
                if (exception != null)
                {
                    await _loggingService.LogError($"Failed to remove book {bookId} from cart for user {username}", caller, exception, correlationId);
                    response.message = "Failed to remove book from cart.";
                    response.status = false;
                    response.data = 0;
                }
                else
                {
                    await _loggingService.LogInformation($"Book {bookId} removed from cart for user {username}", caller, correlationId);
                    response.message = "Book removed from cart successfully.";
                    response.status = true;
                    response.data = result;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"An error occurred while removing book {bookId} from cart for user {username}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = 0;
            }

            return response;
        }
        public async Task<GenericResponse<int>> ClearCartAsync(string username, string caller, string correlationId)
        {
            caller += "||" + nameof(ClearCartAsync);
            GenericResponse<int> response = new GenericResponse<int>();

            try
            {
                var (result, exception) = await _cartRepository.ClearCartAsync(username);
                if (exception != null)
                {
                    await _loggingService.LogError($"Failed to clear cart for user {username}", caller, exception, correlationId);
                    response.message = "Failed to clear cart.";
                    response.status = false;
                    response.data = 0;
                }
                else
                {
                    await _loggingService.LogInformation($"Cart cleared successfully for user {username}", caller, correlationId);
                    response.message = "Cart cleared successfully.";
                    response.status = true;
                    response.data = result;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"An error occurred while clearing cart for user {username}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = 0;
            }

            return response;
        }
        public async Task<GenericResponse<IEnumerable<BookDTO>> >GetBooksInCart(string username, string caller, string correlationId)
        {
            caller += "||" + nameof(GetBooksInCart);
            GenericResponse<IEnumerable<BookDTO>> response = new GenericResponse<IEnumerable<BookDTO>>();

            try
            {
                var checks = await GetCartByUserAsync(username, caller, correlationId);
                if(checks.data != null)
                {
                    var books=new List<BookDTO>();
                    foreach (var cart in checks.data)
                    {
                       var (book,bookException)=await _bookRepository.GetBookByIdAsync(cart.bookId);
                        books.Add(book);
                    }
                    await _loggingService.LogInformation($"No book in cart. {username}", caller, correlationId);
                    response.message = $"{books.Count()} books returned";
                    response.status = true;
                    response.data = books;
                }
                else
                {
                    await _loggingService.LogInformation($"No book in cart. {username}", caller, correlationId);
                    response.message = "No book in cart.";
                    response.status = true;
                    response.data = null;
                
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"An error occurred while clearing cart for user {username}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = null;
            }

            return response;
        }
    }
}
