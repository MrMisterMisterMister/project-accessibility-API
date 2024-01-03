using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace API.Extensions
{
    public static class AuthorizationExtensionService
    {
        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services,
            IConfiguration config)
        {
            var multiSchemePolicy = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();

            // Configure the global authorization policy
            services.AddAuthorization(opt => opt.DefaultPolicy = multiSchemePolicy);
            
            return services;
        }
    }
}