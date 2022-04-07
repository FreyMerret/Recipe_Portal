using Microsoft.EntityFrameworkCore;
using RecipePortal.Db.Entities;

namespace RecipePortal.Db.Context.Context;

public class MainDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Recipe> Recipes { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Category>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<Category>().Property(x => x.Title).HasMaxLength(50);
        modelBuilder.Entity<Category>().HasIndex(x => x.Title).IsUnique();

        modelBuilder.Entity<Comment>().ToTable("comments");
        //добавить связь с автором
        modelBuilder.Entity<Comment>().Property(x => x.CommentText).IsRequired();
        modelBuilder.Entity<Comment>().Property(x => x.CommentText).HasMaxLength(500);
        modelBuilder.Entity<Comment>().HasOne(x => x.Recipe).WithMany(x => x.Comments).HasForeignKey(x => x.RecipeId).OnDelete(DeleteBehavior.Cascade); //if we delete recipe, then we delete all it's comments

        modelBuilder.Entity<Ingredient>().ToTable("ingredients");
        modelBuilder.Entity<Ingredient>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<Ingredient>().Property(x => x.Name).HasMaxLength(50);
        modelBuilder.Entity<Ingredient>().HasMany(x => x.Resipes).WithMany(x => x.Ingredients).UsingEntity(t => t.ToTable("recipes_ingredients"));

        modelBuilder.Entity<Recipe>().ToTable("recipes");
        //добавить связь с автором и все остальное
        modelBuilder.Entity<Recipe>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<Recipe>().Property(x => x.Title).HasMaxLength(100);
        modelBuilder.Entity<Recipe>().HasOne(x => x.Category).WithMany(x => x.Resipes).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<Comment>().HasOne(x => x.Author).WithOne(x => x.Detail).HasPrincipalKey<AuthorDetail>(x => x.Id);
        //modelBuilder.Entity<Recipe>().HasOne(x => x.Category).WithMany(x => x.Resipes).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        //modelBuilder.Entity<Ingredient>().HasMany(x => x.Resipes).WithMany(x => x.Ingredients).UsingEntity(t => t.ToTable("recipes_ingredients"));
    }
}
