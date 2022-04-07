using RecipePortal.API;
using RecipePortal.API.Configuration;
using RecipePortal.Settings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
{
    loggerConfiguration
    .Enrich.WithCorrelationId()
    .ReadFrom.Configuration(hostBuilderContext.Configuration);
});

// Add services to the container.

var settings = new ApiSettings(new SettingsSource());

// Configure services
var services = builder.Services;

#region services
services.AddHttpContextAccessor();

services.AddAppDbContext(settings);

services.AddAppHealthCheck();

services.AddAppVersions();

services.AddAppSwagger(settings);

services.AddAppCors();

services.AddAppServices();

services.AddAppAuth(settings);

services.AddControllers().AddValidator();

services.AddRazorPages();

services.AddAutoMappers();
#endregion

var app = builder.Build();

#region apps
Log.Information("Starting up");

app.UseAppMiddlewares();

app.UseStaticFiles();

app.UseRouting();

app.UseAppCors();

app.UseAppHealthCheck();

app.UseSerilogRequestLogging();

app.UseAppSwagger();

app.UseAppAuth();

app.MapRazorPages();

app.MapControllers();

app.UseAppDbContext();
#endregion

app.Run();
