//using Microsoft.AspNetCore.Http;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using System;
//using System.Diagnostics.Eventing.Reader;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Claims;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace BookManagementService.API.Filter
//{


//    public class RoleBasedSwaggerFilter : IDocumentFilter
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly HttpClient _httpClient;
//        public RoleBasedSwaggerFilter(IHttpContextAccessor httpContextAccessor, HttpClie    nt httpClient)
//        {
//            _httpContextAccessor = httpContextAccessor;
//            _httpClient = httpClient;
//        }

//        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
//        {
//            var httpContext = _httpContextAccessor.HttpContext;
//            if (httpContext == null) return;
//            string userRole = "";
//            bool isAdmin = false;
//            bool isUser = false;
//            var userRoles = httpContext.Session.GetString("Role");
//            if (userRoles == null || userRoles.Length == 0)
//            {
//                isAdmin = false;
//                isUser = false;
//            }
//            else
//            {
//          isAdmin = userRoles.IndexOf("Admin", StringComparison.OrdinalIgnoreCase) >= 0; 
//            isUser = userRoles.IndexOf("User", StringComparison.OrdinalIgnoreCase) >= 0; 
//            }
//            foreach (var path in swaggerDoc.Paths.ToList())
//            {
//                var isAdminEndpoint = path.Key.Contains("UserAppSettings", StringComparison.OrdinalIgnoreCase)
//                      || path.Key.Contains("Logging", StringComparison.OrdinalIgnoreCase);

//                var isStaffEndpoint = path.Key.Contains("CardWebService", StringComparison.OrdinalIgnoreCase);
//                var isHide = path.Key.Contains("ConfrimPasswordReset", StringComparison.OrdinalIgnoreCase);

//                    if (isHide)
//                    {
//                        swaggerDoc.Paths.Remove(path.Key);
//                    }
//                    // Hide "admin" endpoints for non-admins
//                    if ((isAdminEndpoint )&& !isAdmin)
//                    {
//                        swaggerDoc.Paths.Remove(path.Key);
//                    }
//                    // Hide "staff" endpoints for users without "admin" or "user" roles
//                    else if (isStaffEndpoint && !(isAdmin || isUser))
//                    {
//                        swaggerDoc.Paths.Remove(path.Key);
//                    }
//            }
//        }
//        private string[] GetUserRoles(HttpContext httpContext)
//        {
//            // Create a request to the authorization endpoint
//            var correctedUrl = GetBaseUrl() + "/Authenticate/User/check";

//            var request = new HttpRequestMessage(HttpMethod.Get, correctedUrl);
//            var response = _httpClient.Send(request);

//            if (response.IsSuccessStatusCode)
//            {
//                var content = response.Content.ReadAsStringAsync().Result;
//                dynamic jsonContent = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
//                return jsonContent.Roles.ToObject<string[]>();
//            }

//            return Array.Empty<string>();
//        }

//        public string ReplaceText(string originalText, string textToRemove)
//        {
//            // Remove the specific text
//            string modifiedText = originalText.Replace(textToRemove, string.Empty);

//            return modifiedText;
//        }
//        public string GetBaseUrl()
//        {
//            var request = _httpContextAccessor.HttpContext?.Request;
//            if (request == null)
//            {
//                return string.Empty;
//            }

//            // Build the base URL
//            var baseUrl = $"{request.Scheme}://{request.Host}";
//            return baseUrl;
//        }
//    }

//}
