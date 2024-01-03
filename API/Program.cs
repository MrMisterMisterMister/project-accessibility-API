using API.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.SeedData;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.
services.AddEndpointsApiExplorer();
services.AddControllers();

services.AddApplicationServices(config);
services.AddIdentityServices(config);
services.AddAuthorizationServices(config);


builder.Services.AddAuthentication( options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Use Google authentication
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["GoogleAuth:ClientId"]; // Retrieve ClientId from appsettings.json
    options.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"]; // You may store ClientSecret similarly
});

var app = builder.Build();

// Configure the HTTP request pipeline. HTTPS will be handled by NGINX
app.UseMiddleware<ExceptionMiddleware>();
app.UseForwardedHeaders();
app.UseCors("CorsPolicy");
app.UseAuthentication();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// creates scope for services to setup local database and discards itself
using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

if (app.Environment.IsDevelopment())
{
    try
    {
        var databaseContext = serviceProvider.GetRequiredService<DataContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        await databaseContext.Database.MigrateAsync(); // executes dotnet ef update on the latest migrations
        await Seed.SeedAll(databaseContext, userManager); // inserts seed data into the database
    }
    catch (Exception e)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occured during migration");
    }
}

app.Run();