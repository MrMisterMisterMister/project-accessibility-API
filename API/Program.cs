using API.Extensions;
using API.Middleware;
using API.Services;
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


var app = builder.Build();

// Configure the HTTP request pipeline. HTTPS will be handled by NGINX
app.UseMiddleware<ExceptionMiddleware>();

// Enable X-Content-Type-Options header to prevent MIME-sniffing
app.UseXContentTypeOptions();

// Set Referrer-Policy header to control how much information the browser includes with navigations away from a document
app.UseReferrerPolicy(opt => opt.NoReferrer());

// Enable XXssProtection header to defend against cross-site scripting attacks
app.UseXXssProtection(opt => opt.EnabledWithBlockMode());

// Enable X-Frame-Options header to prevent your site from being framed, defending against clickjacking
app.UseXfo(opt => opt.Deny());

// Enable Content Security Policy (CSP) to protect your site from various types of attacks, allowing only approved content
app.UseCsp(opt => opt
    .BlockAllMixedContent()
    .StyleSources(s => s.Self())
    .FontSources(s => s.Self())
    .FormActions(s => s.Self())
    .FrameAncestors(s => s.Self())
    .ScriptSources(s => s.Self())
);

app.Use(async (context, next) =>
{
    // Middleware to set Strict-Transport-Security header, enforcing the use of HTTPS
    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");

    // Middleware to set Permissions-Policy header, controlling features and APIs that can be used in the browser
    context.Response.Headers.Add("Permissions-Policy", "geolocation=(); midi=(); notifications=(); push=(); sync-xhr=(); accelerometer=(); gyroscope=(); magnetometer=(); payment=(); usb=(); vr=(); camera=(); microphone=(); speaker=(); vibrate=(); ambient-light-sensor=(); autoplay=(); encrypted-media=(); execute-clipboard=(); document-domain=(); fullscreen=(); imagecapture=(); lazyload=(); legacy-image-formats=(); oversized-images=(); unoptimized-lossy-images=(); unoptimized-lossless-images=(); unsized-media=(); vertical-scroll=(); web-share=(); xr-spatial-tracking=();");

    await next.Invoke();
});

app.UseForwardedHeaders();
app.UseCors("CorsPolicy");

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
        // Get DataContext service
        var databaseContext = serviceProvider.GetRequiredService<DataContext>();

        // Drop database
        // databaseContext.Database.EnsureDeleted();

        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roleService = new RoleService(userManager, roleManager);

        // Executes dotnet ef update on the latest migrations
        await databaseContext.Database.MigrateAsync();

        // Seed roles
        await roleService.SeedRoles();

        // Inserts seed data into the database
        await Seed.SeedAll(databaseContext, userManager);
    }
    catch (Exception e)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occured during migration");
    }
}

app.Run();