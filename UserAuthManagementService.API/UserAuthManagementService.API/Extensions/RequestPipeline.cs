
using UserAuthManagementService.API.middleware;
using UserAuthManagementService.Service.GRPCServices;
using Serilog;


namespace ThirdPartyCardAPIs.API.Extensions
{
    public static class RequestPipeline
    {
        public static async void ConfigureRequestPipeline(this WebApplication app, IWebHostEnvironment env)
        {
            try
            {
                //app.UseMiddleware<ContentSecurityPolicyMiddleware>();
                app.UseMiddleware<PermissionsPolicyMiddlware>();
                app.UseMiddleware<ReferrerPolicyMiddleware>();
                app.UseMiddleware<StrictTransportPolicyMiddleware>();
                app.UseMiddleware<XContentTypeOptionsMiddleware>();
                app.UseMiddleware<XFrameOptionsMiddleware>();
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseMiddleware<CorrelationIdMiddleware>();
                app.UseMiddleware<InputValidationMiddleware>();
                //var grpcClient = app.Services.GetRequiredService<GrpcClient>();
                //await grpcClient.SendRequestMessageAsync("ProcessBook", "BookService", "BookPayload");
                app.MapGrpcService<RequestMessagerService>();
                //app.MapGet("/", () => "gRPC Server is running.");
                app.UseHttpsRedirection();

                    app.UseHsts(); // Default HSTS settings
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {

            }
}
    }
}
