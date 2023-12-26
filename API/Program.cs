using API.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.SeedData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); // Services should be added in the extension method 

var app = builder.Build();

// Configure the HTTP request pipeline. Https will be configured by NGINX
app.UseForwardedHeaders();
app.MapControllers();

using var scope = app.Services.CreateScope(); // creates scope for services to setup local database and discards itself
var services = scope.ServiceProvider;

try
{
    var databaseContext = services.GetRequiredService<DatabaseContext>();
    await databaseContext.Database.MigrateAsync(); // executes dotnet ef update on the latest migrations
    await Seed.SeedAll(databaseContext); // inserts seed data into the database
}
catch (Exception e)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "An error occured during migration");
}

app.Run();