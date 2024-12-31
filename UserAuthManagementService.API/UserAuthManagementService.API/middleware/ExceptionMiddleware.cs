using UserAuthManagementService.Service.GenericServices.Interface;
using System.Net;
using System.Text;
using UserAuthManagementService.Data.Repository.Interface;
using UserAuthManagementService.Domain.DTO.Common;
using UserAuthManagementService.Service.GenericServices.Interface;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    // Inject only the necessary dependencies (IInboundLogRepository)
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, IInboundLogRepository inboundLogRepository,IEncryptionService encryptionService)
    {
        var ip = "";
            ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault(); ;

            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress?.ToString();
            }
            var userHostAddress = context.Connection.RemoteIpAddress?.ToString();

            ip += "|" + userHostAddress;

        context.Request.EnableBuffering();

        if (context.Request.Path.ToString() == "/" || context.Request.Path.ToString().Contains("/swagger/"))
        {
            await _next(context);
        }
        else
        {
            _logger.LogInformation($"*********This is the middleware : requestPathArr {context.Request.Path.ToString()}");
            var responseStr = string.Empty;

            // Instantiate InboundLog manually
            var inboundLog = new InboundLog
            {
                Ip=ip,
                APICalled = "AuditTrail API",
                APIMethod = context.Request.Path.ToString(),
                RequestDateTime = DateTime.UtcNow.AddHours(1)
            };

            var inboundLogStr = System.Text.Json.JsonSerializer.Serialize(inboundLog);
            _logger.LogInformation($"*********This is the service : inboundLogStr {inboundLogStr}");
            var message = string.Empty;
            var originalResponse = context.Response.Body;

            using (var tempMemoryStream = new MemoryStream())
            {
                try
                {
                    byte[] byteArr;
                    context.Response.Body = tempMemoryStream;
                    using var streamReader = new StreamReader(context.Request.Body, Encoding.UTF8);
                    var requestPayload = await streamReader.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(requestPayload))
                    {
                        string payloadStringify = System.Text.Json.JsonSerializer.Serialize(requestPayload);
                        inboundLog.RequestDetails = encryptionService.Encrypt(payloadStringify,"cinfotech");
                        inboundLogStr = System.Text.Json.JsonSerializer.Serialize(inboundLog);
                        _logger.LogInformation($"********This is the service : inboundLogStr {inboundLogStr}");
                    }
                    byteArr = Encoding.UTF8.GetBytes(requestPayload);
                    context.Request.Body = new MemoryStream(byteArr);
                    context.Request.Body.Position = 0;

                    await _next(context);

                    tempMemoryStream.Position = 0;
                    responseStr = Encoding.UTF8.GetString(tempMemoryStream.ToArray());

                    if (!string.IsNullOrEmpty(responseStr))
                    {
                        inboundLog.ResponseDetails = responseStr;
                    }
                    inboundLogStr = System.Text.Json.JsonSerializer.Serialize(inboundLog);
                    _logger.LogInformation($"********This is the service : inboundLogStr after response {inboundLogStr}");
                }
                catch (Exception ex)
                {
                    message = "Your request can not be processed at the moment, please try again later";
                    var response = new GenericResponse<HttpStatusCode>() { status = false, data = HttpStatusCode.InternalServerError, message = message };
                    responseStr = System.Text.Json.JsonSerializer.Serialize(response);
                    _logger.LogError(ex, ex.Source, ex.InnerException, ex.Message);
                    context.Response.ContentType = "application/json";
                    inboundLog.ExceptionDetails = $@"******* MESSAGE: {ex.Message}*********
                    ********** STACKTRACE: {ex.StackTrace}**********";
                }
                inboundLog.CorrelationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
                context.Response.Body = originalResponse;
                inboundLog.ResponseDateTime = DateTime.UtcNow;

                // Persist the log using the repository
                inboundLogRepository.CreateInboundLog(inboundLog);
                await context.Response.WriteAsync(responseStr);
            }
        }
    }
}
