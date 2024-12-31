
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UserAuthManagementService.Domain.DTO.Common;

namespace UserAuthManagementService.API.middleware
{
    public class JsonValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JsonValidationMiddleware> _logger;

        public JsonValidationMiddleware(RequestDelegate next, ILogger<JsonValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
            {
                context.Request.EnableBuffering();

                using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    List<string> message = new List<string>
                {
                    "Ensure there is no trailing comma",
                    "Ensure that array elements are separated by commas",
                    "Ensure that all special characters are properly escaped",
                    "Ensure that the JSON structure matches the expected schema",
                    "Ensure that all string values are properly enclosed in double quotes",
                };

                    if (!IsValidJson(body, out string exceptionMessage))
                    {
                        _logger.LogWarning("Invalid JSON received");
                        if (!string.IsNullOrEmpty(exceptionMessage))
                        {
                            message.Add(exceptionMessage);
                        }

                        var response =new GenericResponse<string>(){data=null, status=false, message="Invalid JSON format."};
                        var responseStr = JsonConvert.SerializeObject(response);
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(responseStr);
                        return;
                    }
                }
            }

            await _next(context);
        }

        private bool IsValidJson(string json, out string exceptionMessage)
        {
            exceptionMessage = string.Empty;

            if (string.IsNullOrEmpty(json) || json == "string")
            {
                exceptionMessage = "Empty or invalid JSON.";
                return false;
            }

            try
            {
                // Check for common issues before parsing
                if (json.Contains("\": ,"))
                {
                    exceptionMessage = "One or more properties have missing values.";
                    return false;
                }

                JToken.Parse(json);
                return true;
            }
            catch (JsonReaderException ex)
            {
                exceptionMessage = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                exceptionMessage = "An error occurred while parsing JSON.";
                return false;
            }
        }
    }


}
