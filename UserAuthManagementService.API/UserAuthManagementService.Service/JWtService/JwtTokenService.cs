using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using UserAuthManagementService.Domain.Entity;
using Microsoft.Extensions.Configuration;
using UserAuthManagementService.Service.GenericServices.Interface;


namespace UserAuthManagementService.Service.JWtService
{

    public class JwtTokenService
    {
        private readonly string _secretKey;
        private readonly IConfiguration _configuration;
        private readonly ILoggingService _loggingService;

        public JwtTokenService(IConfiguration configuration, ILoggingService loggingService)
        {
            _configuration = configuration;
            _secretKey = "hgjhsoiwlka;ft[pkg[le[g]5]gl=]4p;eg];]=]p;=tyeeveheuieeiebge";
            _loggingService = loggingService;
        }
        public string GenerateJwtToken(UserDTO user, string caller, string corelationId)
        {
            try
            {

                caller = caller + "||" + nameof(GenerateJwtToken);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.role),
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim("aud", "http://localhost:1222"), // Additional audience claim
                    new Claim("aud", "http://localhost:1223"),  // Additional audience claim
                    new Claim("aud", "http://localhost:1224")  // Additional audience claim
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
            issuer: "http://localhost:1234",
            audience: "http://localhost:1234", // Single audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(480),
                    signingCredentials: creds);
                _loggingService.LogInformation("TokenGotSuccesful", caller, corelationId);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Token Request is not Succesful", caller, ex, corelationId);
                return null;
            }

        }
        public ClaimsPrincipal? ValidateToken(string token, string caller, string corelationId)
        {
            caller = caller + "||" + nameof(GenerateJwtToken);

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.");
            string[] _validAudiences = new[] { "http://localhost:1234", "http://localhost:1222", "http://localhost:1223" };
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:1234",
                ValidateAudience = false, // Disable default audience validation
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ClockSkew = TimeSpan.Zero // Optional: Reduce token expiration tolerance
            };

            try
            {
                // Validate the token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Manually validate audiences
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    if (!_validAudiences.Contains(jwtToken.Issuer))
                    {
                        _loggingService.LogWarning("Invalid audience", caller, corelationId);
                        throw new SecurityTokenInvalidAudienceException("Invalid audience");
                    }
                }
                _loggingService.LogInformation("Token validation is Succesful", caller, corelationId);
                return principal;
            }
            catch (SecurityTokenException ex)
            {
                _loggingService.LogError($"Token validation failed: {ex.Message}", caller, ex, corelationId);
                return null;
            }
        }
    }

}
