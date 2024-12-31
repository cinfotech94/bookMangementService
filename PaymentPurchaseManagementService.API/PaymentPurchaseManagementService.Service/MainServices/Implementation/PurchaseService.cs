using PaymentPurchaseManagementService.Data.Repository.Implementation;
using PaymentPurchaseManagementService.Data.Repository.Interface;
using PaymentPurchaseManagementService.Domain.DTO.Common;
using PaymentPurchaseManagementService.Domain.DTO.Request;
using PaymentPurchaseManagementService.Domain.Entity;
using PaymentPurchaseManagementService.Service.GenericServices.Interface;
using PaymentPurchaseManagementService.Service.GRPCServices;
using PaymentPurchaseManagementService.Service.RabbitMQServices;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace PaymentPurchaseManagementService.Service.MainServices.Implementation
{
    public class PurchaseService: IPurchaseService
    {
        private readonly ILoggingService _loggingService;
        private readonly IPurchaseRepository _purchaseRepository;
     private readonly BookRequestPublish _bookRequestPublish;
     private readonly UserRequestPublish _userRequestPublish;
private readonly MyGrpcClient _myGrpcClient;
        public PurchaseService(MyGrpcClient myGrpcClient, BookRequestPublish bookRequestPublish,UserRequestPublish userRequestPublish, ILoggingService loggingService,IPurchaseRepository purchaseRepository)
        {
            _loggingService = loggingService;
            _purchaseRepository = purchaseRepository;
            _bookRequestPublish = bookRequestPublish;
            _userRequestPublish = userRequestPublish;
            _myGrpcClient = myGrpcClient;

        }
        public async Task<GenericResponse<int>> AddBookToPurchaseAsync(PurchaseDTO purchase, string caller, string correlationId)
        {
            caller += "||" + nameof(AddBookToPurchaseAsync);
            GenericResponse<int> response = new GenericResponse<int>();

            try
            {

                var auditDTo = new AuditDTO()
                {
                    correlationId = Guid.Parse(correlationId),
                    type = nameof(AddBookToPurchaseAsync),
                    description = $"trying to AddBookToPurchaseAsync for {purchase.username} and book{purchase.bookId}",
                };
               PaymentPurchaseManagementService.Domain.DTO.Common. RequestMessage mqRequestMessageAudit = new PaymentPurchaseManagementService.Domain.DTO.Common.RequestMessage()
                {
                    caller = "payment-queue",
                    MethodName = "Audit",
                    Payload = auditDTo,
                    RequestTime = DateTime.Now
                };
                await _userRequestPublish.SendRequestMessageAsync(caller, mqRequestMessageAudit);
                // Send the request and get the response
                var bookResponse = await _myGrpcClient.SendBookRequestBookMessageAsync("GetBookById",caller,purchase.bookId);
                GenericResponse<BookDTO> genericResponseBook = System.Text.Json.JsonSerializer.Deserialize<GenericResponse<BookDTO>>(bookResponse.Payload);
                var book = genericResponseBook.data;
                if (book == null)
                {
                    response.message = "book not exist";
                    response.status = false;
                    response.data = 0;
                    return response;
                }
                var books = await GetPurchaseByUserAsync(purchase.username, caller, correlationId);
                if (books.data.Any(u => u.bookId == purchase.bookId))
                {
                    response.message = "existing purchase already";
                    response.status = false;
                    response.data = 0;
                    return response;
                }
                var decreaseBookResponse = await _myGrpcClient.SendBookRequestUserMessageAsync("GetUser", caller, $"{purchase.username}||{1}");
                GenericResponse<UserResponseDto> genericResponseGetUser = System.Text.Json.JsonSerializer.Deserialize<GenericResponse<UserResponseDto>>(decreaseBookResponse.Payload);
                var userResponseDto = genericResponseGetUser.data;
                if (userResponseDto == null)
                {
                    await _loggingService.LogInformation($"Failed to add book to purchase for user {purchase.username}", caller, correlationId);
                    response.message = "Failed to add book to purchase.the user doesnot exist";
                    response.status = false;
                    response.data = 0;
                    return response;

                }
                else
                {
                    if(userResponseDto.balance < book.price)
                    {
                        await _loggingService.LogInformation($"Failed to add book to purchase for user {purchase.username}", caller, correlationId);
                        response.message = "Failed to add book to purchase your balance is lower tht the price of the book";
                        response.status = false;
                        response.data = 0;
                        return response;
                    }
                }
                var UpdateBalanaceResponse = await _myGrpcClient.SendBookRequestBookMessageAsync("UpdateBalanace", caller, purchase.username+"||"+ book.price);
                GenericResponse<string> genericResponseUpdateBalanace = System.Text.Json.JsonSerializer.Deserialize<GenericResponse<string>>(bookResponse.Payload);
                var reductionStatus = genericResponseUpdateBalanace.status;
                if (reductionStatus)
                {
                    var (result, exception) = await _purchaseRepository.AddBookToPurchaseAsync(purchase);
                    if (exception != null)
                    {
                        await _loggingService.LogError($"Failed to add book to purchase for user {purchase.username}", caller, exception, correlationId);
                        response.message = "Failed to add book to purchase.";
                        response.status = false;
                        response.data = 0;
                    }
                    else
                    {
                        auditDTo.description = $"Add BookToPurchaseAsync ia succesful for {purchase.username} and book{purchase.bookId}";
                        PaymentPurchaseManagementService.Domain.DTO.Common.RequestMessage mqRequestMessageauditDTo1 = new PaymentPurchaseManagementService.Domain.DTO.Common.RequestMessage()
                        {
                            caller = "payment-queue",
                            MethodName = "Audit",
                            Payload = auditDTo,
                            RequestTime = DateTime.Now
                        };
                        await _userRequestPublish.SendRequestMessageAsync(caller, mqRequestMessageauditDTo1);
                        PaymentPurchaseManagementService.Domain.DTO.Common.RequestMessage mqRequestMessageDecreaseBook = new PaymentPurchaseManagementService.Domain.DTO.Common.RequestMessage()
                        {
                            caller = "payment-queue",
                            MethodName = "DecreaseBook",
                            Payload = $"{purchase.bookId}||{1}",
                            RequestTime = DateTime.Now
                        };
                        await _bookRequestPublish.SendRequestMessageAsync(caller, mqRequestMessageDecreaseBook);
                        PaymentPurchaseManagementService.Domain.DTO.Common.RequestMessage mqRequestMessageRemoveBookFromCart = new PaymentPurchaseManagementService.Domain.DTO.Common.RequestMessage()
                        {
                            caller = "payment-queue",
                            MethodName = "RemoveBookFromCart",
                            Payload = $"{purchase.username}||{purchase.bookId}",
                            RequestTime = DateTime.Now
                        };
                        await _bookRequestPublish.SendRequestMessageAsync(caller, mqRequestMessageRemoveBookFromCart);
                        await _loggingService.LogInformation($"Book added to cart successfully for user {purchase.username}", caller, correlationId);
                        response.message = "Book added to purchase successfully.";
                        response.status = true;
                        response.data = result;
                    }
                }
                else
                {
                    await _loggingService.LogInformation($"Failed to add book to purchase for user {purchase.username}", caller, correlationId);
                    response.message = "Failed to add book to purchase ";
                    response.status = false;
                    response.data = 0;
                    return response;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"An error occurred while adding book to purchase for user {purchase.username}", caller, ex, correlationId);
                response.message = ex.Message;
                response.status = false;
                response.data = 0;
            }

            return response;
        }
        public async Task<GenericResponse<IEnumerable<PurchaseDTO>>> GetPurchaseByUserAsync(string username, string caller, string correlationId)
        {
            caller += "||" + nameof(GetPurchaseByUserAsync);
            GenericResponse<IEnumerable<PurchaseDTO>> response = new GenericResponse<IEnumerable<PurchaseDTO>>();

            try
            {
                var (cartItems, exception) = await _purchaseRepository.GetPurchaseByUserAsync(username);
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
        public async Task<GenericResponse<IEnumerable<BookDTO>>> PurchaseCart(string username, string caller, string correlationId)
        {
            caller += "||" + nameof(PurchaseCart);
            GenericResponse<IEnumerable<BookDTO>> response = new GenericResponse<IEnumerable<BookDTO>>();

            try
            {
                var bookResponse = await _myGrpcClient.SendBookRequestBookMessageAsync("GetBooksInCart", caller, username);
                GenericResponse<IEnumerable<BookDTO>> books = System.Text.Json.JsonSerializer.Deserialize<GenericResponse<IEnumerable<BookDTO>>>(bookResponse.Payload);
                if (books != null)
                {
                    double price=0;
                    var availableBook=new List<BookDTO>();
                    foreach (var book in books.data)
                    {
                        if(book.quantity>0)
                        {
                            price = price + book.price;
                            availableBook.Add(book);
                        }
                        else
                        {
                            price = price;
                        }
                    }
                    var decreaseBookResponse = await _myGrpcClient.SendBookRequestUserMessageAsync("GetUser", caller, $"{username}||{1}");
                    GenericResponse<UserResponseDto> genericResponseGetUser = System.Text.Json.JsonSerializer.Deserialize<GenericResponse<UserResponseDto>>(decreaseBookResponse.Payload);
                    var userResponseDto = genericResponseGetUser.data;
                    if (userResponseDto == null)
                    {
                        await _loggingService.LogInformation($"Failed to add book to purchase for user {username}", caller, correlationId);
                        response.message = "Failed to add book to purchase.the user doesnot exist";
                        response.status = false;
                        response.data = null;
                        return response;

                    }
                    else
                    {
                        if (userResponseDto.balance < price)
                        {
                            await _loggingService.LogInformation($"Failed to add book to purchase for user {username}", caller, correlationId);
                            response.message = "Failed to add book to purchase your balance is lower tht the price of the books";
                            response.status = false;
                            response.data = null;
                            return response;
                        }
                    }
                    var UpdateBalanaceResponse = await _myGrpcClient.SendBookRequestBookMessageAsync("UpdateBalanace", caller, username + "||" + price);
                    GenericResponse<string> genericResponseUpdateBalanace = System.Text.Json.JsonSerializer.Deserialize<GenericResponse<string>>(bookResponse.Payload);
                    var reductionStatus = genericResponseUpdateBalanace.status;
                    if(reductionStatus)
                    {
                        foreach (var book in availableBook)
                        {

                            PaymentPurchaseManagementService.Domain.DTO.Common.RequestMessage mqRequestMessageDecreaseBook = new PaymentPurchaseManagementService.Domain.DTO.Common.RequestMessage()
                            {
                                caller = "payment-queue",
                                MethodName = "DecreaseBook",
                                Payload = $"{book.id}||{1}",
                                RequestTime = DateTime.Now
                            };
                            await _bookRequestPublish.SendRequestMessageAsync(caller, mqRequestMessageDecreaseBook);
                        }
                        await _loggingService.LogInformation($"No book in cart. {username}", caller, correlationId);
                        response.message = $"{availableBook.Count()} books returned";
                        response.status = true;
                        response.data = availableBook;
                    }
                    else
                    {
                        await _loggingService.LogInformation($"Failed to add book to purchase for user {username}", caller, correlationId);
                        response.message = "Failed to add book to purchase ";
                        response.status = false;
                        response.data = null;
                        return response;
                    }
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
