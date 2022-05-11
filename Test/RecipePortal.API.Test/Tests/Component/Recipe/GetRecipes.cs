namespace RecipePortal.API.Test.Tests.Component.Recipe;

using RecipePortal.API.Test.Common.Extensions;
using RecipePortal.API.Controllers.Recipes.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public partial class RecipeIntegrationTest
{
    [Test]
    public async Task GetRecipes_ValidParameters_Authenticated_OkResponse()
    {
        var accessToken = await AuthenticateUser_ReadAndWriteRecipesScope();
        var url = Urls.GetRecipes(0,10);
        var response = await apiClient.Get(url, accessToken);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var recipes_from_api = await response.ReadAsObject<IEnumerable<RecipeResponse>>();

        await using var context = await DbContext();
        var recipes_from_db = context.Recipes.AsEnumerable();

        Assert.AreEqual(recipes_from_db.Count(), recipes_from_api.Count());
    }

    [Test]
    public async Task GetRecipes_NegativeParameters_OkResponse()
    {
        var accessToken = await AuthenticateUser_ReadAndWriteRecipesScope();
        var url = Urls.GetRecipes(-1, -1);
        var response = await apiClient.Get(url, accessToken);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var recipes_from_api = await response.ReadAsObject<IEnumerable<RecipeResponse>>();

        Assert.AreEqual(0, recipes_from_api.Count());
    }

    [Test]
    public async Task GetRecipes_Unauthorized()
    {
        var url = Urls.GetRecipes();
        var response = await apiClient.Get(url);
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Test]  
    public async Task GetRecipes_EmptyScope_Forbidden()
    {
        var accessToken = await AuthenticateUser_EmptyScope();
        var url = Urls.GetRecipes();
        var response = await apiClient.Get(url, accessToken);

        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
