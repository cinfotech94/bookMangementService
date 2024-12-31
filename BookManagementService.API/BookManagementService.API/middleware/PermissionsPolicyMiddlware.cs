namespace BookManagementService.API.middleware
{
    public class PermissionsPolicyMiddlware
    {
        private readonly RequestDelegate _next;

        public PermissionsPolicyMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Add the X-Frame-Options header to the response
            context.Response.Headers.Add("Permissions-Policy", "geolocation=(), microphone=()");
            await _next(context);
        }
    }
}
