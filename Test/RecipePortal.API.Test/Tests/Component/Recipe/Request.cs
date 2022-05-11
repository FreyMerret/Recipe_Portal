namespace RecipePortal.API.Test.Tests.Component.Recipe;

using RecipePortal.API.Controllers.Recipes.Models;
using System.Collections.Generic;

public partial class RecipeIntegrationTest
{
    public static AddRecipeRequest AddRecipeRequest(int categoryId, string title, string description, string text)
    {
        return new AddRecipeRequest()
        {
            CategoryId = categoryId,
            Title = title,
            Description = description,
            Text = text,
            RecipeCompositionFields = new List<AddRecipeCompositionFieldRequest>() 
            {
                new AddRecipeCompositionFieldRequest()
                {
                    IngredientId = 1,
                    Quantity = "100гр"
                }
            }
        };
    }

    public static UpdateRecipeRequest UpdateRecipeRequest(int categoryId, string title, string description, string text)
    {
        return new UpdateRecipeRequest()
        {
            CategoryId = categoryId,
            Title = title,
            Description = description,
            Text = text
        };
    }
}
