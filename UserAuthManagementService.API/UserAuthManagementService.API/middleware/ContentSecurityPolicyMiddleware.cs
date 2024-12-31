namespace UserAuthManagementService.API.middleware
{
    public class ContentSecurityPolicyMiddleware
    {
        private readonly RequestDelegate _next;

        public ContentSecurityPolicyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; img-src 'self' https:; script-src 'self'; style-src 'self' 'unsafe-inline'");
            await _next(context);
        }
    }

}
