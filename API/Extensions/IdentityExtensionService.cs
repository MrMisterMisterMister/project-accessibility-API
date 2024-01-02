using System.Text;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    public static class IdentityExtensionService
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddIdentityCore<User>(opt =>
            {
                // Not too complicated to save my brain
                // will add more options later
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>();

            // Retrieveing security key for JWT token generation and validation
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            (config["TokenKey"]!));

            // Adding JWT authentication services
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    // Configuring token validation parameters for JWT bearer authentication
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false, // MUST change when going into production
                        ValidateAudience = false // MUST change when going into production
                    };
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

            // Registering TokenService as a scoped service within the application's service collection
            // And adding the httpcontext as a service.
            services.AddHttpContextAccessor();
            services.AddScoped<TokenService>();

            return services;
        }
    }
}