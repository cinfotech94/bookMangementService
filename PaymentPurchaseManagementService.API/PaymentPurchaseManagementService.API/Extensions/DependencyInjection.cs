

using Microsoft.AspNetCore.Mvc;
using PaymentPurchaseManagementService.Domain;
using FluentValidation.AspNetCore;
using PaymentPurchaseManagementService.Domain.DTO.Request;
using PaymentPurchaseManagementService.Domain.DTO.Common;
using FluentValidation;
using PaymentPurchaseManagementService.API.Extensions;
using PaymentPurchaseManagementService.Data;
using Asp.Versioning;
using PaymentPurchaseManagementService.service;


namespace ThirdPartyCardAPIs.API.Extensions
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                services.AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ApiVersionReader = new HeaderApiVersionReader();
                    options.ReportApiVersions = true;
                })
.AddApiExplorer(options =>
{
options.GroupNameFormat = "'v'V"; // This will group by API version e.g. v1, v2
options.SubstituteApiVersionInUrl = true; // Adds version to URL
});
                services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
                services.AddHttpClient();
                services.AddHttpContextAccessor();
                //services.AddFluentValidationAutoValidation();
                //services.AddFluentValidationClientsideAdapters();
                services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = false ;
            });

                services.AddControllers();
                //.AddFluentValidation(config =>
                //{
                //    // Automatically register validators in the current assembly
                //    config.RegisterValidatorsFromAssemblyContaining<Program>();

                //    // Explicitly register validators from other assemblies
                //    config.RegisterValidatorsFromAssemblyContaining<ValidateCardPIN_GORequestValidator>();
                //    config.RegisterValidatorsFromAssemblyContaining<BulkCardIssuanceValidator>();
                //    config.RegisterValidatorsFromAssemblyContaining<ChnagePasswordRequestValidator>();
                //    config.RegisterValidatorsFromAssemblyContaining<ModifyCardLimitRequestValidator>();
                //    config.RegisterValidatorsFromAssemblyContaining<GetCardStatementRequestValidator>();
                //    config.RegisterValidatorsFromAssemblyContaining<PinSelectReqValidator>();
                //    config.RegisterValidatorsFromAssemblyContaining<GetCountryByCurrCodeParameterValidator>();
                //    config.RegisterValidatorsFromAssemblyContaining<GetIssuanceStatusParametersValidator>();
                //    config.RegisterValidatorsFromAssemblyContaining<GetActiveCardsWithAccountsIdValidator>();
                //});

                // Alternatively, scan entire assemblies
                //services.AddValidatorsFromAssembly(typeof(ValidateCardPIN_GORequestValidator).Assembly);
                //services.AddValidatorsFromAssembly(typeof(BulkCardIssuanceValidator).Assembly);
                //services.AddValidatorsFromAssembly(typeof(ChnagePasswordRequestValidator).Assembly);
                //services.AddValidatorsFromAssembly(typeof(ModifyCardLimitRequestValidator).Assembly);
                //services.AddValidatorsFromAssembly(typeof(GetCardStatementRequestValidator).Assembly);
                //services.AddValidatorsFromAssembly(typeof(PinSelectReqValidator).Assembly);
                //services.AddValidatorsFromAssembly(typeof(GetCountryByCurrCodeParameterValidator).Assembly);
                //services.AddValidatorsFromAssembly(typeof(GetIssuanceStatusParametersValidator).Assembly);
                //services.AddValidatorsFromAssembly(typeof(GetActiveCardsWithAccountsIdValidator).Assembly);
                //services.AddEndpointsApiExplorer();
                services.AddDistributedMemoryCache();
                services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(30);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true; // Make the session cookie essential
                });
                services.AddMemoryCache();
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen();
                services.AddSwaggerConfiguration();
                services.AddGrpc();
                services.AddDataLayerService(configuration);
            services.AddServiceLayer(configuration);
            }
            catch (Exception ex)
            {

            }
}
    }
}
