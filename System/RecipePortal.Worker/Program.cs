using RecipePortal.Settings;
using RecipePortal.Worker;
using Serilog;

// Configure application
var builder = WebApplication.CreateBuilder(args);

// Settings for the initial configuration
var settings = new WorkerSettings(new SettingsSource());

// Logger
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
{
    loggerConfiguration
    .Enrich.WithCorrelationId()
    .ReadFrom.Configuration(hostBuilderContext.Configuration);
});

// Configure services
var services = builder.Services;

services.AddAppDbContext(settings);
services.AddHttpContextAccessor();
services.AddAppHealthCheck();
services.RegisterServices();


// Start application

var app = builder.Build();

app.UseAppHealthCheck();

app.StartTaskExecutor();
Log.Debug("StartTaskExecutor");

app.Run();

