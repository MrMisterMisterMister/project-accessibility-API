using Application.Core;
using Application.UserHandlers;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationExtensionService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
        {
            // This is where all the services should be placed for cleaner clode

            services.AddDbContext<DataContext>(opt =>
                opt.UseMySql(config.GetConnectionString("MyAccessibleDatabase"),
                    new MySqlServerVersion(new Version(8, 0, 35))
                )
            );

            // Adds cors policy so http request can be made. Needs to be changed when going in production
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("https://clodsire.nl");
                });
            });

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUser.Handler).Assembly)); // Adds mediatr service
            services.AddAutoMapper(typeof(MappingProfiles).Assembly); // Adds automapper service

            return services;
        }
    }
}