
using BookManagementService.API.middleware;
using BookManagementService.Service.GRPCServices;
using Dapper;
using Elastic.Transport;
using MongoDB.Driver.Core.Configuration;
using Npgsql;
using Serilog;


namespace ThirdPartyCardAPIs.API.Extensions
{
    public static class RequestPipeline
    {
        public static async void ConfigureRequestPipeline(this WebApplication app, IWebHostEnvironment env)
        {
            try
            {
                //await CheckDatabaseTableExist(app.Configuration);
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
                if (!app.Environment.IsDevelopment())
                {

                    app.UseHsts();
                }
                app.UseHttpsRedirection();

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
        private static async Task CheckDatabaseTableExist(IConfiguration configuration)
        {
            try
            {
                using (var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Step 1: Create Books table if it does not exist
                    var createBooksTableQuery = @"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM information_schema.tables 
        WHERE table_name = 'books' AND table_schema = 'public'
    ) THEN
        CREATE TABLE Books (
            Id UUID PRIMARY KEY,
            Title VARCHAR(255),
            ISBN VARCHAR(255),
            Author VARCHAR(255),
            PublicationYear VARCHAR(4),
            TimeAdded TIMESTAMP,
            Genre VARCHAR(255),
            Quantity INT DEFAULT 1,
            Price FLOAT,
            Pages INT,
            Description VARCHAR(1000),
            Category VARCHAR(255),
            NoClick INT DEFAULT 0,
            NoOfPurchase INT DEFAULT 0,
            NoOfCart INT DEFAULT 0
        );
    END IF;
END $$;";

                    await connection.ExecuteAsync(createBooksTableQuery);
                    //Console.WriteLine("Books table checked and created if necessary.");

                    // Step 2: Create Carts table if it does not exist
                    var createCartsTableQuery = @"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM information_schema.tables 
        WHERE table_name = 'carts' AND table_schema = 'public'
    ) THEN
        CREATE TABLE Carts (
            Username VARCHAR(255),
            BookId UUID,
            PRIMARY KEY (Username, BookId)
        );
    END IF;
END $$;";

                    await connection.ExecuteAsync(createCartsTableQuery);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
            }
    }
    }
}
