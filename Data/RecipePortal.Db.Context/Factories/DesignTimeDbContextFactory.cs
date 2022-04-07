using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using RecipePortal.Db.Context.Context;

namespace RecipePortal.Db.Context.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
{
    public MainDbContext CreateDbContext(string[] args)
    {
        var cfg = new ConfigurationBuilder()
            .AddJsonFile("appsettings.design.json")
            .Build();

        var options = new DbContextOptionsBuilder<MainDbContext>()
            .UseSqlServer(cfg.GetConnectionString("MainDbContext"))
            .Options;
        
        return new MainDbContextFactory(options).Create();
    }
}
