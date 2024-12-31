using Serilog;
using ThirdPartyCardAPIs.API.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args)
                .AddSerilog();
    builder.Logging.AddFilter("FluentValidation", LogLevel.Debug);
    builder.Services.AddHttpClient();
    builder.Services.AddServices(builder.Configuration);
    builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));
    if (!builder.Environment.IsDevelopment())
    {
        builder.Services.AddHttpsRedirection(options =>
        {
            options.HttpsPort = 443;
        });
    }
    builder.Host.UseSerilog();
    var app = builder.Build();
    app.ConfigureRequestPipeline(builder.Environment);

}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, ex.Source ?? string.Empty, ex.InnerException, ex.Message, ex.ToString());
}
finally
{
    Log.CloseAndFlush();
}
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseRouting();
//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
