namespace UserAuthManagementService.API.middleware
{
    public class StrictTransportPolicyMiddleware
    {
        private readonly RequestDelegate _next;

        public StrictTransportPolicyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Add the X-Frame-Options header to the response
            context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
            await _next(context);
        }
    }
}
