using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Entities;

namespace RecipePortal.Db.Context.Setup;

public static class DbSeed
{


    private static void Seed(MainDbContext context)
    {
        if (context.Recipes.Any() || context.Ingredients.Any() || context.Categories.Any())
            return;


        #region AddCategories

        var categ1 = new Category()
        {
            Title = "Первые блюда",
            Recipes = new List<Recipe>() { }
        };
        context.Categories.Add(categ1);

        var categ2 = new Category()
        {
            Title = "Вторые блюда",
            Recipes = new List<Recipe>() { }
        };
        context.Categories.Add(categ2);

        var categ3 = new Category()
        {
            Title = "Десерты",
            Recipes = new List<Recipe>() { }
        };
        context.Categories.Add(categ3);

        var categ4 = new Category()
        {
            Title = "Салаты",
            Recipes = new List<Recipe>() { }
        };
        context.Categories.Add(categ4);

        var categ5 = new Category()
        {
            Title = "Напитки",
            Recipes = new List<Recipe>() { }
        };
        context.Categories.Add(categ5);

        var categ6 = new Category()
        {
            Title = "Приправы",
            Recipes = new List<Recipe>() { }
        };
        context.Categories.Add(categ6);

        var categ7 = new Category()
        {
            Title = "Маринады",
            Recipes = new List<Recipe>() { }
        };
        context.Categories.Add(categ7);
        #endregion

        #region AddIngredients
        var ing1 = new Ingredient()
        {
            Name = "Куриное филе",
            RecipeCompositionFields = new List<CompositionField>() { }
        };
        context.Ingredients.Add(ing1);

        var ing2 = new Ingredient()
        {
            Name = "Картофель",
            RecipeCompositionFields = new List<CompositionField>() { }
        };
        context.Ingredients.Add(ing2);

        var ing3 = new Ingredient()
        {
            Name = "Рис",
            RecipeCompositionFields = new List<CompositionField>() { }
        };
        context.Ingredients.Add(ing3);

        var ing4 = new Ingredient()
        {
            Name = "Лук",
            RecipeCompositionFields = new List<CompositionField>() { }
        };
        context.Ingredients.Add(ing4);
        #endregion

        #region AddUsers
        var user1 = new User()
        {
            UserName = "TestAuthor",
            Name = "Павел",
            Surname = "Подгорный",
            Email = "",
            PasswordHash = "Passw0rd".GetHashCode().ToString(),
            Status = 0,
            Recipes = new List<Recipe>() { },
            Comments = new List<Comment>() { }
        };
        context.Users.Add(user1);
        #endregion

        #region AddRecipes
        var r1 = new Recipe()
        {
            Title = "Картошечка с курочкой",
            Description = "Жареная картошка с куриным филе.",
            Text = "Картошка жареная в масле. Куриное филе, нарезанное ломтиками и обжаренное с душистым перцем.",
            CompositionFields = new List<CompositionField>()
            {
                new CompositionField()
                {
                    Ingredient = ing1,
                    Quantity = "500 грамм"
                },
                new CompositionField()
                {
                    Ingredient = ing2,
                    Quantity = "1000 грамм"
                }
            },
            Author = user1,
            Category = categ2,
            Comments = new List<Comment>() { }
        };
        context.Recipes.Add(r1);

        var r2 = new Recipe()
        {
            Title = "Куриный суп",
            Description = "Деревенский суп с курицей по рецепту бабули.",
            Text = "Обжариваем лук, добавляем курицу. Жарим 5 минут, добавляем воду, картофель, лук. Варим до готовности.",
            CompositionFields = new List<CompositionField>()
            {
                new CompositionField()
                {
                    Ingredient = ing1,
                    Quantity = "500 грамм"
                },
                new CompositionField()
                {
                    Ingredient = ing2,
                    Quantity = "500 грамм"
                },
                new CompositionField()
                {
                    Ingredient = ing3,
                    Quantity = "200 грамм"
                },
                new CompositionField()
                {
                    Ingredient = ing4,
                    Quantity = "2 штуки"
                }
            },
            Author = user1,
            Category = categ1,
            Comments = new List<Comment>() { }
        };
        context.Recipes.Add(r2);
        #endregion

        #region AddComments
        context.Comments.Add(new Comment()
        {
            Author = user1,
            CommentText = "Пальчики оближешь!",
            Recipe = r1
        });
        #endregion

        context.SaveChanges();
    }

    public static void Execute(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>();
        using var context = factory.CreateDbContext();

        Seed(context);
    }
}
