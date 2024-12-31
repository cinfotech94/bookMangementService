
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookManagementService.API.Extensions
{
    public static class ConfigureAuth
    {
        public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder applicationBuilder)
        {
            try
            {
                var secretKey = applicationBuilder.Configuration["JwtSettings:SecretKey"];
                var audience = applicationBuilder.Configuration["JwtSettings:Audience"];
                var issuer = applicationBuilder.Configuration["JwtSettings:Issuer"];
                applicationBuilder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    };
                });

                applicationBuilder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                    options.AddPolicy("User", policy => policy.RequireRole("User"));
                    options.AddPolicy("App", policy => policy.RequireRole("App"));
                });
                return applicationBuilder;
            }
            catch(Exception ex)
            {
                return applicationBuilder;

            }

        }
    }
}
