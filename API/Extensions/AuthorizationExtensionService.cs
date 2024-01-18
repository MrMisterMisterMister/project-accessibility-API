using Domain;
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
            services.AddAuthorization(opt =>
            {
                opt.DefaultPolicy = multiSchemePolicy;

                // Add "Admin" policy
                opt.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireRole(nameof(RoleTypes.Admin));
                });

                // Add "PanelMember" policy
                opt.AddPolicy("PanelMemberPolicy", policy =>
                {
                    policy.RequireRole(nameof(RoleTypes.PanelMember));
                });

                // Add "PanelMember" policy
                opt.AddPolicy("CompanyPolicy", policy =>
                {
                    policy.RequireRole(nameof(RoleTypes.PanelMember));
                });
            });

            return services;
        }
    }
}