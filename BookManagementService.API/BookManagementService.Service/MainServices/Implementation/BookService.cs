using BookManagementService.Data.Repository.Interface;
using BookManagementService.Domain.DTO.Common;
using BookManagementService.Domain.DTO.Request;
using BookManagementService.Service.GenericServices.Interface;
using BookManagementService.Service.MainServices.Interface;
using BookManagementService.Service.RabbitMQServices;

namespace BookManagementService.Service.MainServices.Implementation
{
    public class BookServices : IBookService
    {
        private readonly ILoggingService _loggingService;
        private readonly IBookRepository _bookRepository;

        public BookServices(RequestPublish serviceClient, ILoggingService loggingService, IBookRepository bookRepository)
        {
            _loggingService = loggingService;
            _bookRepository = bookRepository;
        }

        public async Task<GenericResponse<int>> AddBookAsync(BookDTO book, string caller, string correlationId)
        {
            caller += "||" + nameof(AddBookAsync);
            GenericResponse<int> response = new GenericResponse<int>();
            try
            {
                var (result, exception) = await _bookRepository.AddBookAsync(book);
                if (exception != null)
                {
                    response.message = "Add book failed";
                    response.status = false;
                    response.data = result;
                    await _loggingService.LogError($"Failed to add book with ISBN: {book.ISBN}", caller, exception, correlationId);
                }
                else
                {
                    response.message = "Book added successfully";
                    response.status = true;
                    response.data = result;
                    await _loggingService.LogInformation($"Book added with ISBN: {book.ISBN}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                response.message = "Exception occurred while adding the book";
                response.status = false;
                response.data = 0;
                await _loggingService.LogError($"Exception occurred while adding book with ISBN: {book.ISBN}", caller, ex, correlationId);
            }
            return response;
        }

        public async Task<GenericResponse<BookDTO>> GetBookByIdAsync(Guid id, string caller, string correlationId)
        {
            caller += "||" + nameof(GetBookByIdAsync);
            GenericResponse<BookDTO> response = new GenericResponse<BookDTO>();
            try
            {
                var (book, exception) = await _bookRepository.GetBookByIdAsync(id);
                if (exception != null || book == null)
                {
                    response.message = "Book not found";
                    response.status = false;
                    response.data = null;
                    await _loggingService.LogWarning($"Book not found with ID: {id}", caller, correlationId);
                }
                else
                {
                    response.message = "Book retrieved successfully";
                    response.status = true;
                    response.data = book;
                    await _loggingService.LogInformation($"Book retrieved with ID: {id}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                response.message = "Exception occurred while retrieving the book";
                response.status = false;
                response.data = null;
                await _loggingService.LogError($"Exception occurred while retrieving book with ID: {id}", caller, ex, correlationId);
            }
            return response;
        }

        public async Task<GenericResponse<BookDTO>> GetBookByISBNAsync(string isbn, string caller, string correlationId)
        {
            caller += "||" + nameof(GetBookByISBNAsync);
            GenericResponse<BookDTO> response = new GenericResponse<BookDTO>();
            try
            {
                var (book, exception) = await _bookRepository.GetBookByISBNAsync(isbn);
                if (exception != null || book == null)
                {
                    response.message = "Book not found";
                    response.status = false;
                    response.data = null;
                    await _loggingService.LogWarning($"Book not found with ISBN: {isbn}", caller, correlationId);
                }
                else
                {
                    response.message = "Book retrieved successfully";
                    response.status = true;
                    response.data = book;
                    await _loggingService.LogInformation($"Book retrieved with ISBN: {isbn}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                response.message = "Exception occurred while retrieving the book";
                response.status = false;
                response.data = null;
                await _loggingService.LogError($"Exception occurred while retrieving book with ISBN: {isbn}", caller, ex, correlationId);
            }
            return response;
        }

        public async Task<GenericResponse<List<BookDTO>>> GetAllBooksAsync(string caller, string correlationId, bool ascending = false, int pageNumber = 1, int pageSize = 10, string sortBy = "noClick")
        {
            caller += "||" + nameof(GetAllBooksAsync);
            GenericResponse<List<BookDTO>> response = new GenericResponse<List<BookDTO>>();
            try
            {
                var (books, totalRecords, totalPages, exception) = await _bookRepository.GetAllBooksAsync(ascending, pageNumber, pageSize, sortBy);
                if (exception != null)
                {
                    response.message = "Failed to retrieve books";
                    response.status = false;
                    response.data = books;
                    await _loggingService.LogError($"Failed to retrieve books list", caller, exception, correlationId);
                }
                else
                {
                    response.message = "Books retrieved successfully";
                    response.status = true;
                    response.data = books;
                    await _loggingService.LogInformation($"Books retrieved successfully", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                response.message = "Exception occurred while retrieving books";
                response.status = false;
                response.data = new List<BookDTO>();
                await _loggingService.LogError($"Exception occurred while retrieving books", caller, ex, correlationId);
            }
            return response;
        }

        public async Task<GenericResponse<List<BookDTO>>> SearchBooksAsync(string searchText, string caller, string correlationId, int pageNumber = 1, int pageSize = 10, bool ascending = false, string sortBy = "noClick")
        {
            caller += "||" + nameof(SearchBooksAsync);
            GenericResponse<List<BookDTO>> response = new GenericResponse<List<BookDTO>>();
            try
            {
                var (books, totalRecords, totalPages, exception) = await _bookRepository.SearchBooksAsync(searchText, pageNumber, pageSize, ascending, sortBy);
                if (exception != null)
                {
                    response.message = "Failed to search books";
                    response.status = false;
                    response.data = books;
                    await _loggingService.LogError($"Failed to search books with query: {searchText}", caller, exception, correlationId);
                }
                else
                {
                    response.message = "Books search successful";
                    response.status = true;
                    response.data = books;
                    await _loggingService.LogInformation($"Books search successful with query: {searchText}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                response.message = "Exception occurred during book search";
                response.status = false;
                response.data = new List<BookDTO>();
                await _loggingService.LogError($"Exception occurred during book search with query: {searchText}", caller, ex, correlationId);
            }
            return response;
        }

        public async Task<GenericResponse<int>> UpdateBookAsync(BookDTO book, string caller, string correlationId)
        {
            caller += "||" + nameof(UpdateBookAsync);
            GenericResponse<int> response = new GenericResponse<int>();
            try
            {
                var (result, exception) = await _bookRepository.UpdateBookAsync(book);
                if (exception != null)
                {
                    response.message = "Failed to update book";
                    response.status = false;
                    response.data = result;
                    await _loggingService.LogError($"Failed to update book with ISBN: {book.ISBN}", caller, exception, correlationId);
                }
                else
                {
                    response.message = "Book updated successfully";
                    response.status = true;
                    response.data = result;
                    await _loggingService.LogInformation($"Book updated with ISBN: {book.ISBN}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                response.message = "Exception occurred while updating the book";
                response.status = false;
                response.data = 0;
                await _loggingService.LogError($"Exception occurred while updating book with ISBN: {book.ISBN}", caller, ex, correlationId);
            }
            return response;
        }

        public async Task<GenericResponse<int>> DeleteBookAsync(Guid id, string caller, string correlationId)
        {
            caller += "||" + nameof(DeleteBookAsync);
            GenericResponse<int> response = new GenericResponse<int>();
            try
            {
                var (result, exception) = await _bookRepository.DeleteBookAsync(id);
                if (exception != null)
                {
                    response.message = "Failed to delete book";
                    response.status = false;
                    response.data = result;
                    await _loggingService.LogError($"Failed to delete book with ID: {id}", caller, exception, correlationId);
                }
                else
                {
                    response.message = "Book deleted successfully";
                    response.status = true;
                    response.data = result;
                    await _loggingService.LogInformation($"Book deleted with ID: {id}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                response.message = "Exception occurred while deleting the book";
                response.status = false;
                response.data = 0;
                await _loggingService.LogError($"Exception occurred while deleting book with ID: {id}", caller, ex, correlationId);
            }
            return response;
        }

        public async Task<GenericResponse<int>> DeleteBookByISBNAsync(string isbn, string caller, string correlationId)
        {
            caller += "||" + nameof(DeleteBookByISBNAsync);
            GenericResponse<int> response = new GenericResponse<int>();
            try
            {
                var (result, exception) = await _bookRepository.DeleteBookByISBNAsync(isbn);
                if (exception != null)
                {
                    response.message = "Failed to delete book";
                    response.status = false;
                    response.data = result;
                    await _loggingService.LogError($"Failed to delete book with ISBN: {isbn}", caller, exception, correlationId);
                }
                else
                {
                    response.message = "Book deleted successfully";
                    response.status = true;
                    response.data = result;
                    await _loggingService.LogInformation($"Book deleted with ISBN: {isbn}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                response.message = "Exception occurred while deleting the book";
                response.status = false;
                response.data = 0;
                await _loggingService.LogError($"Exception occurred while deleting book with ISBN: {isbn}", caller, ex, correlationId);
            }
            return response;
        }
        public async Task<GenericResponse<List<BookDTO>>> GetBooksByAuthorAsync(string author, string caller, string correlationId, int pageNumber = 1, int pageSize = 10, bool ascending = false, string sortBy = "noClick")
        {
            caller += "||" + nameof(GetBooksByAuthorAsync);
            GenericResponse<List<BookDTO>> response = new GenericResponse<List<BookDTO>>();

            try
            {
                // Call the repository method to get books by the author
                var (books, totalItems, totalPages, exception) = await _bookRepository.GetBooksByAuthorAsync(author, pageNumber, pageSize, ascending, sortBy);

                if (exception != null)
                {
                    // Log error if any exception is encountered during the retrieval
                    await _loggingService.LogError($"Failed to retrieve books for author {author}", caller, exception, correlationId);
                    response.message = "An error occurred while fetching books.";
                    response.status = false;
                    response.data = null;
                }
                else
                {
                    // Successfully retrieved books
                    response.message = "Success";
                    response.status = true;
                    response.data = books;
                    // Log success message
                    await _loggingService.LogInformation($"Successfully fetched books for author {author}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                // Log unexpected error
                await _loggingService.LogError($"Error occurred while fetching books for author {author}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = null;
            }

            return response;
        }
        public async Task<GenericResponse<bool>> IsBookAvailableAsync(Guid id, string caller, string correlationId)
        {
            caller += "||" + nameof(IsBookAvailableAsync);
            GenericResponse<bool> response = new GenericResponse<bool>();

            try
            {
                // Get the book by its ID
                var (book, exception) = await _bookRepository.GetBookByIdAsync(id);

                if (exception != null || book == null)
                {
                    // If the book is not found or there's an error, return failure
                    await _loggingService.LogError($"Book with ID {id} not found or an error occurred", caller, exception, correlationId);
                    response.message = "Book not found or an error occurred.";
                    response.status = false;
                    response.data = false;
                }
                else
                {
                    // Check if the book's quantity is greater than 0
                    bool isAvailable = book.quantity > 0;

                    if (isAvailable)
                    {
                        response.message = "Book is available.";
                        response.status = true;
                    }
                    else
                    {
                        response.message = "Book is out of stock.";
                        response.status = false;
                    }

                    response.data = isAvailable;

                    // Log the availability check
                    await _loggingService.LogInformation($"Book availability checked for ID {id}: {isAvailable}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                await _loggingService.LogError($"Error occurred while checking availability for book ID {id}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = false;
            }

            return response;
        }
        public async Task<GenericResponse<int>> DecreaseBookQuantityAsync(Guid id, int quantityToDecrease, string caller, string correlationId)
        {
            caller += "||" + nameof(DecreaseBookQuantityAsync);
            GenericResponse<int> response = new GenericResponse<int>();

            try
            {
                // Get the book by its ID
                var (book, exception) = await _bookRepository.GetBookByIdAsync(id);

                if (exception != null || book == null)
                {
                    // If the book is not found or there's an error, return failure
                    await _loggingService.LogError($"Book with ID {id} not found or an error occurred while decreasing quantity", caller, exception, correlationId);
                    response.message = "Book not found or an error occurred.";
                    response.status = false;
                    response.data = 0;
                }
                else
                {
                    if (book.quantity >= quantityToDecrease)
                    {
                        // Decrease the book's quantity
                        book.quantity -= quantityToDecrease;

                        // Update the book's quantity in the database
                        var (update, updateException) = await _bookRepository.UpdateBookAsync(book);

                        if (updateException != null)
                        {
                            await _loggingService.LogError($"Failed to update quantity for book ID {id}", caller, updateException, correlationId);
                            response.message = "Failed to update book quantity.";
                            response.status = false;
                            response.data = book.quantity;
                        }
                        else
                        {
                            response.message = $"Successfully decreased quantity for book ID {id}. New quantity: {book.quantity}.";
                            response.status = true;
                            response.data = book.quantity;

                            // Log the successful quantity reduction
                            await _loggingService.LogInformation($"Book quantity decreased for ID {id}. New quantity: {book.quantity}", caller, correlationId);
                        }
                    }
                    else
                    {
                        // If the book's quantity is less than the amount to decrease, return failure
                        response.message = "Not enough stock to decrease by the specified amount.";
                        response.status = false;
                        response.data = book.quantity;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                await _loggingService.LogError($"Error occurred while decreasing quantity for book ID {id}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = 0;
            }

            return response;
        }
        public async Task<GenericResponse<double>> CheckBookPriceAsync(Guid id, string caller, string correlationId)
        {
            caller += "||" + nameof(CheckBookPriceAsync);
            GenericResponse<double> response = new GenericResponse<double>();

            try
            {
                // Get the book by its ID
                var (book, exception) = await _bookRepository.GetBookByIdAsync(id);

                if (exception != null || book == null)
                {
                    // If the book is not found or there's an error, return failure
                    await _loggingService.LogError($"Book with ID {id} not found or an error occurred while checking price", caller, exception, correlationId);
                    response.message = "Book not found or an error occurred.";
                    response.status = false;
                    response.data = 0;
                }
                else
                {
                    // Return the book's price
                    response.message = $"Price of the book with ID {id}: {book.price}";
                    response.status = true;
                    response.data = book.price;

                    // Log the successful price check
                    await _loggingService.LogInformation($"Price checked for book ID {id}: {book.price}", caller, correlationId);
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                await _loggingService.LogError($"Error occurred while checking price for book ID {id}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = 0;
            }

            return response;
        }

    }


}
