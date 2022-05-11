namespace RecipePortal.API.Test.Tests.Unit.Recipe;

using RecipePortal.RecipeService.Models;
using System;
using System.Collections.Generic;

public partial class RecipeUnitTest
{
    public static UpdateRecipeModel UpdateRecipeModel(Guid authorId, int recipeId, int categoryId, string title, string description, string text)
    {
        return new UpdateRecipeModel()
        {
            RequestAuthor = authorId,

            RecipeId = recipeId,

            CategoryId = categoryId,

            Title = title,

            Description = description,

            Text = text
        };
    }

    public static AddRecipeModel AddRecipeModel(Guid authorId, int categoryId, string title, string description, string text,int ingredientId, string quantity)
    {
        return new AddRecipeModel()
        {
            AuthorId = authorId,

            CategoryId = categoryId,

            Title = title,

            Description = description,

            Text = text,
            
            RecipeCompositionFields = new List<AddRecipeComposititonFieldModel>()
            {
                new AddRecipeComposititonFieldModel()
                { 
                    IngredientId = ingredientId,
                    Quantity = quantity
                }
            }
        };
    }
}
