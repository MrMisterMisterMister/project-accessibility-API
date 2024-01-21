using System.Text;
using API.Services;
using Application.Interfaces;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
            .AddRoles<IdentityRole>() // adds role based authorization
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            // Retrieveing security key for JWT token generation and validation
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]!));

            // On DO droplet
            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            // (Environment.GetEnvironmentVariable("TokenKey")!));

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
                        ValidateAudience = false, // MUST change when going into production
                        ValidateLifetime = true, // validates life time of a token (default window is every 5 minutes)
                        ClockSkew = TimeSpan.Zero // sets default window to 0
                    };
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

            // Registering TokenService and UserAccessor
            // as a scoped service within the application's service collection
            // And adding the httpcontext as a service.
            services.AddHttpContextAccessor();
            services.AddScoped<TokenService>();
            services.AddScoped<IUserAccessor, UserAccessor>(); // this will make it available to be injected in application handlers

            return services;
        }
    }
}