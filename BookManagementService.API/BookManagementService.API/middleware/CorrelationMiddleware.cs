namespace BookManagementService.API.middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context?.Request?.Headers?.Add("X-Correlation-ID", correlationId);
            }
            context?.Response?.Headers?.Add("X-Correlation-ID", correlationId);
            await _next(context);
        }
    }
}
