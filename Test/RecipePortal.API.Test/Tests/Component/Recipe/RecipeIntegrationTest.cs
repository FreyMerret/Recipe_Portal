namespace RecipePortal.API.Test.Tests.Component.Recipe;

using RecipePortal.API.Test.Common;
using RecipePortal.Db.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public partial class RecipeIntegrationTest : ComponentTest
{
    [SetUp]
    public async Task SetUp()
    {
        await using var context = await DbContext();

        context.Categories.RemoveRange(context.Categories);
        context.Ingredients.RemoveRange(context.Ingredients);        
        context.Users.RemoveRange(context.Users);
        context.Recipes.RemoveRange(context.Recipes);
        context.Comments.RemoveRange(context.Comments);
        context.SaveChanges();

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
        var u1 = new User()
        {
            Name = "Test",
            Surname = "User",
            UserName = "testUser",
            Email = "tst@tst.ru",
            EmailConfirmed = true
        };
        userManager.CreateAsync(u1, "1234");
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
            Author = u1,
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
            Author = u1,
            Category = categ1,
            Comments = new List<Comment>() { }
        };
        context.Recipes.Add(r2);
        #endregion

        #region AddComments
        context.Comments.Add(new Comment()
        {
            Author = u1,
            CommentText = "Пальчики оближешь!",
            Recipe = r1
        });
        #endregion

    }

    [TearDown]
    public async override Task TearDown()
    {
        await using var context = await DbContext();
        context.Categories.RemoveRange(context.Categories);
        context.Ingredients.RemoveRange(context.Ingredients);
        context.Users.RemoveRange(context.Users);
        context.Recipes.RemoveRange(context.Recipes);
        context.Comments.RemoveRange(context.Comments);
        context.SaveChanges();
        await base.TearDown();
    }

    protected static class Urls
    {
        public static string GetRecipes(int? offset = null, int? limit = null)
        {

            if (offset is null && limit is null)
                return $"/api/v1/recipes";
            List<string> queryParameters = new List<string>();

            if (offset.HasValue)
            {
                queryParameters.Add($"offset={offset}");
            }

            if (limit.HasValue)
            {
                queryParameters.Add($"limit={limit}");
            }

            var queryString = string.Join("&", queryParameters);
            return $"/api/v1/recipes?{queryString}";
        }

        public static string GetRecipe(string id) => $"/api/v1/recipes/{id}";

        public static string DeleteRecipe(string id) => $"/api/v1/recipes/{id}";

        public static string UpdateRecipe(string id) => $"/api/v1/recipes/{id}";

        public static string AddRecipe => $"/api/v1/recipes";
    }

    public static class Scopes
    {
        public static string ReadRecipes => "offline_access recipes_read";

        public static string WriteRecipes => "offline_access recipes_write";

        public static string ReadAndWriteRecipes => "offline_access recipes_read recipes_write";

        public static string Empty => "offline_access";
    }

    public async Task<string> AuthenticateUser_ReadAndWriteRecipesScope()
    {
        var user = GetTestUser();
        var tokenResponse = await AuthenticateTestUser(user.Username, user.Password, Scopes.ReadAndWriteRecipes);
        return tokenResponse.AccessToken;
    }

    public async Task<string> AuthenticateUser_EmptyScope()
    {
        var user = GetTestUser();
        var tokenResponse = await AuthenticateTestUser(user.Username, user.Password, Scopes.Empty);
        return tokenResponse.AccessToken;
    }

    public async Task<User> GetExistedAuthor()
    {
        await using var context = await DbContext();
        if (context.Users.Count() == 0)
        {
            var u1 = new User()
            {
                Name = "Test",
                Surname = "User",
                UserName = "testUser",
                Email = "tst@tst.ru",
                EmailConfirmed = true

            };
            userManager.CreateAsync(u1, "1234");
        }

        await using var context1 = await DbContext();
        var author = context1.Users.AsEnumerable().First();
        return author;
    }

    public async Task<string> GetNotExistedAuthorNickname()
    {
        return "NotExistedAuthor";
    }

}
