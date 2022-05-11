namespace RecipePortal.API.Test.Tests.Component.Recipe;

using RecipePortal.API.Test.Common.Extensions;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public partial class RecipeIntegrationTest
{


    [Test]
    [TestCaseSource(typeof(Generator), nameof(Generator.InvalidCategoryId))]
    public async Task AddRecipe_InvalidAuthor_Authenticated_BadRequest(int authorId)
    {
        var accessToken = await AuthenticateUser_ReadAndWriteRecipesScope();
        var url = Urls.AddRecipe;

        var request = AddRecipeRequest(Generator.InvalidCategoryId.First(), Generator.ValidTitles.First(), Generator.ValidDescriptions.First(), Generator.ValidTexts.First());
        var response = await apiClient.PostJson(url, request, accessToken);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Test]
    [TestCaseSource(typeof(Generator), nameof(Generator.InvalidTitles))]
    public async Task AddRecipe_InvalidTitle_Authenticated_BadRequest(string title)
    {
        var accessToken = await AuthenticateUser_ReadAndWriteRecipesScope();
        var url = Urls.AddRecipe;

        var request = AddRecipeRequest(Generator.InvalidCategoryId.First(), title, Generator.ValidDescriptions.First(), Generator.ValidTexts.First());
        var response = await apiClient.PostJson(url, request, accessToken);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Test]
    public async Task AddRecipe_Unauthorized()
    {
        var url = Urls.AddRecipe;

        var request = AddRecipeRequest(Generator.ValidCategoryId.First(), Generator.ValidTitles.First(), Generator.ValidDescriptions.First(), Generator.ValidTexts.First());
        var response = await apiClient.PostJson(url, request, null);
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Test]
    public async Task AddRecipe_Forbidden()
    {
        var accessToken = await AuthenticateUser_EmptyScope();
        var url = Urls.AddRecipe;

        var request = AddRecipeRequest(Generator.ValidCategoryId.First(), Generator.ValidTitles.First(), Generator.ValidDescriptions.First(), Generator.ValidTexts.First());
        var response = await apiClient.PostJson(url, request, accessToken);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
