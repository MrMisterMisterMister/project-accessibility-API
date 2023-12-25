using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationExtensionService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>(opt =>
            {
                opt.UseMySql(config.GetConnectionString("TestDatabase"), new MySqlServerVersion(new Version(8, 0, 35)));
            });
            
            return services;
        }
    }
}