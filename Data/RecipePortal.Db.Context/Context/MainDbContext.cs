using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipePortal.Db.Entities;

namespace RecipePortal.Db.Context.Context;

public class MainDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Composition> Compositions { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_role_owners");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("user_role_claims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");

        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Category>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<Category>().Property(x => x.Title).HasMaxLength(50);
        modelBuilder.Entity<Category>().HasIndex(x => x.Title).IsUnique();

        modelBuilder.Entity<Comment>().ToTable("comments");
        modelBuilder.Entity<Comment>().Property(x => x.CommentText).IsRequired();
        modelBuilder.Entity<Comment>().Property(x => x.CommentText).HasMaxLength(500);
        modelBuilder.Entity<Comment>().HasOne(x => x.Recipe).WithMany(x => x.Comments).HasForeignKey(x => x.RecipeId); //if we delete recipe, then we delete all it's comments
        modelBuilder.Entity<Comment>().HasOne(x => x.Author).WithMany(x => x.Comments).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ingredient>().ToTable("ingredients");
        modelBuilder.Entity<Ingredient>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<Ingredient>().Property(x => x.Name).HasMaxLength(50);
        modelBuilder.Entity<Ingredient>().HasMany(x => x.Composition).WithOne(x => x.Ingredient).HasForeignKey(x => x.IngredientId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Recipe>().ToTable("recipes");
        modelBuilder.Entity<Recipe>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<Recipe>().Property(x => x.Title).HasMaxLength(100);
        modelBuilder.Entity<Recipe>().HasOne(x => x.Category).WithMany(x => x.Recipes).HasForeignKey(x => x.CategoryId); //перекинуть в категории ради интереса
        modelBuilder.Entity<Recipe>().HasOne(x => x.Author).WithMany(x => x.Recipes).HasForeignKey(x => x.AuthorId);

        modelBuilder.Entity<Composition>().ToTable("composition"); //состав
        modelBuilder.Entity<Composition>().HasOne(x => x.Recipe).WithMany(x => x.Composition).HasForeignKey(x => x.RecipeId);


        //modelBuilder.Entity<Comment>().HasOne(x => x.Author).WithOne(x => x.Detail).HasPrincipalKey<AuthorDetail>(x => x.Id);
        //modelBuilder.Entity<Recipe>().HasOne(x => x.Category).WithMany(x => x.Recipes).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        //modelBuilder.Entity<Ingredient>().HasMany(x => x.Recipes).WithMany(x => x.Ingredients).UsingEntity(t => t.ToTable("recipes_ingredients"));
    }
}
