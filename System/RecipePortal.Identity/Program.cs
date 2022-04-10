using RecipePortal.Identity;
using RecipePortal.Identity.Configuration;
using RecipePortal.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = new IS4Settings(new SettingsSource());

var services = builder.Services;

services.AddAppCors();
services.AddAppDbContext(settings.Db);
services.AddHttpContextAccessor();
services.AddAppServices();
services.AddIS4();

var app = builder.Build();


app.UseAppCors();
app.UseStaticFiles();
app.UseRouting();
app.UseAppDbContext();
app.UseIS4();

app.Run();
