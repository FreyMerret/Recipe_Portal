using Microsoft.EntityFrameworkCore;
using RecipePortal.Db.Context.Context;

namespace RecipePortal.Db.Context.Factories;

public class DbContextOptionFactory
{
    public static DbContextOptions<MainDbContext> Create(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<MainDbContext>();
        Configure(connectionString).Invoke(builder);
        return builder.Options;
    }

    //править конфигурацию
    public static Action<DbContextOptionsBuilder> Configure(string connectionString)
    {
        return (builder) => builder.UseSqlServer(connectionString, opt =>
        {
            opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        });
    }
}
