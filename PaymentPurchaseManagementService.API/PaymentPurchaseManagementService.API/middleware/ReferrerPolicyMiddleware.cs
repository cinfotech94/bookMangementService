namespace PaymentPurchaseManagementService.API.middleware
{
    public class ReferrerPolicyMiddleware
    {
        private readonly RequestDelegate _next;

        public ReferrerPolicyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Add the Referrer-Policy header to the response
            context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin"); // Or other policy value
            await _next(context);
        }
    }

}
