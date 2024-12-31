namespace UserAuthManagementService.API.middleware
{
    public class XContentTypeOptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public XContentTypeOptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Add the X-Content-Type-Options header to the response
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            await _next(context);
        }
    }

}
