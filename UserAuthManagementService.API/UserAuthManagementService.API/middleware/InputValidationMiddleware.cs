using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserAuthManagementService.API.middleware
{


    public class InputValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public InputValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Validation for query parameters
            foreach (var key in context.Request.Query.Keys)
            {
                string value = context.Request.Query[key];
                if (!IsInputValid(value))
                {
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("Invalid input detected!");
                    return;
                }
            }

            await _next(context);
        }

        private bool IsInputValid(string input)
        {
            // Check for unwanted CRLF characters
            return !input.Contains("\r") && !input.Contains("\n");
        }
    }
}
