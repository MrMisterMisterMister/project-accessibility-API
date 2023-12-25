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

            services.AddDbContext<DatabaseContext>(opt =>
            {
                opt.UseMySql(config.GetConnectionString("TestDatabase"), new MySqlServerVersion(new Version(8, 0, 35)));
            });
            
            return services;
        }
    }
}