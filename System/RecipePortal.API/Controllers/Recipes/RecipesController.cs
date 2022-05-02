using AutoMapper;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RecipePortal.API.Controllers.Recipes.Models;
using RecipePortal.Common.Security;
using RecipePortal.Db.Entities;
using RecipePortal.RecipeService;
using RecipePortal.RecipeService.Models;
using RecipePortal.UserAccountService;
using RecipePortal.UserAccountService.Models;
using RecipePortal.API.Controllers.UserAccounts.Models;

namespace RecipePortal.API.Controllers.Recipes;

/// <summary>
/// Управление рецептами, ингридиентами, комментариями, подписками на рецепты и комменатрии
/// </summary>
[Route("api/v{version:apiVersion}/recipes")]
[ApiController]
[ApiVersion("1.0")]
public class RecipesController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<RecipesController> logger;
    private readonly IRecipeService recipeService;
    private readonly UserManager<User> userManager;
    private readonly IUserAccountService userAccountService;

    public RecipesController(IMapper mapper, ILogger<RecipesController> logger, IRecipeService recipeService, UserManager<User> userManager, IUserAccountService userAccountService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.recipeService = recipeService;
        this.userManager = userManager; //для работы с пользователями
        this.userAccountService = userAccountService;
    }

    #region --------------------------------------------------Recipes--------------------------------------------------
    /// <summary>
    /// Выдача всех рецептов + фильтры
    /// </summary>
    /// <param name="recipeName">Фильтр по имени. Выдача будет содержать только рецепты, в имени которых содержится введенная строка</param>
    /// <param name="categoryId">Фильтр по категории. Выдача будет содержать только рецепты данной категории (ID категории)</param>
    /// <param name="authorNickname">Фильтр по автору. Выдача будет содержать только рецепты данного автора (Ник автора)</param>
    /// <param name="offset">Количество рецептов, которое надо пропустить в выдаче</param>
    /// <param name="limit">Количество рецептов, которое надо выдать</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesRead)]
    [HttpGet]
    public async Task<IEnumerable<RecipeResponse>> GetRecipes([FromQuery] string recipeName = "", [FromQuery] int categoryId = 0, [FromQuery] string authorNickname = "", [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var recipes = await recipeService.GetRecipes(recipeName, categoryId, authorNickname, offset, limit);
        var response = mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    /// <summary>
    /// Выдача конкретного рецепта
    /// </summary>
    /// <param name="recipeId">ID рецепта</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesRead)]
    [HttpGet("{recipeId}")]
    public async Task<RecipeResponse> GetRecipeById([FromRoute] int recipeId)
    {
        var recipe = await recipeService.GetRecipe(recipeId);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    /// <summary>
    /// Добавление рецепта
    /// </summary>
    /// <param name="request">Форма добавления рецепта</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPost]
    [Authorize]
    public async Task<RecipeResponse> AddRecipe([FromBody] AddRecipeRequest request)
    {
        var model = mapper.Map<AddRecipeModel>(request);
        model.AuthorId = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value); //получение guid авторизованного пользователя из claims запроса
        var recipe = await recipeService.AddRecipe(model);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    /// <summary>
    /// Изменение рецепта (ингридиенты рецепта изменяются отдельным контроллером по одному)
    /// </summary>
    /// <param name="recipeId">ID рецепта, который необходимо изменить</param>
    /// <param name="request">Форма изменения рецепта</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPut("{recipeId}")]
    [Authorize]
    public async Task<RecipeResponse> UpdateRecipe([FromRoute] int recipeId, [FromBody] UpdateRecipeRequest request)
    {
        var model = mapper.Map<UpdateRecipeModel>(request);
        model.RecipeId = recipeId;
        model.RequestAuthor = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        var recipe = await recipeService.UpdateRecipe(model);
        var response = mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    /// <summary>
    /// Удаление рецепта
    /// </summary>
    /// <param name="recipeId">ID рецепта, который необходимо удалить</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpDelete("{recipeId}")]
    [Authorize]
    public async Task<IActionResult> DeleteRecipe([FromRoute] int recipeId)
    {
        var requestAuthor = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        await recipeService.DeleteRecipe(recipeId, requestAuthor);

        return Ok();
    }

    #endregion



    #region -------------------------------------------------Comments--------------------------------------------------

    /// <summary>
    /// Получение комментариев к рецепту
    /// </summary>
    /// <param name="recipeId">ID рецепта, для которого необходимо получить комментарии</param>
    /// <param name="offset">Количество комментариев, которые необходимо пропустить</param>
    /// <param name="limit">Количество комментариев, которые необходимо выдать</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.CommentsRead)]
    [HttpGet("{recipeId}/comments")]
    public async Task<IEnumerable<CommentResponse>> GetComments([FromRoute] int recipeId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var comments = await recipeService.GetComments(recipeId, offset, limit);
        var response = mapper.Map<IEnumerable<CommentResponse>>(comments);

        return response;
    }

    /// <summary>
    /// Добавление комментария к рецепту
    /// </summary>
    /// <param name="recipeId">ID рецепта, куда добавляется комментарий</param>
    /// <param name="request">Форма комментария</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.CommentsWrite)]
    [HttpPost("{recipeId}/comments")]
    [Authorize]
    public async Task<CommentResponse> AddComment([FromRoute] int recipeId, [FromBody] AddCommentRequest request)
    {
        var model = mapper.Map<AddCommentModel>(request);
        model.RecipeId = recipeId;
        model.AuthorId = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        var comment = await recipeService.AddComment(model);
        var response = mapper.Map<CommentResponse>(comment);

        return response;
    }

    /// <summary>
    /// Изменение комментария
    /// </summary>
    /// <param name="commentId">ID изменяемого комментария</param>
    /// <param name="request">Форма для изменения комментария</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.CommentsWrite)]
    [HttpPut("{recipeId}/comments/{commentId}")]
    [Authorize]
    public async Task<CommentResponse> UpdateComment([FromRoute] int commentId, [FromBody] UpdateCommentRequest request)
    {
        var model = mapper.Map<UpdateCommentModel>(request);
        model.CommentId = commentId;
        model.RequestAuthor = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        var comment = await recipeService.UpdateComment(model);
        var response = mapper.Map<CommentResponse>(comment);

        return response;
    }

    /// <summary>
    /// Удаление комментария
    /// </summary>
    /// <param name="commentId">ID удаляемого комментария</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.CommentsWrite)]
    [HttpDelete("{recipeId}/comments/{commentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
    {
        var requestAuthor = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        await recipeService.DeleteComment(commentId, requestAuthor);

        return Ok();
    }
    #endregion



    #region ---------------------------------------------CompositionFields---------------------------------------------

    /// <summary>
    /// Добавление ингридиента в существующий рецепт по ID рецепта
    /// </summary>
    /// <param name="recipeId">ID рецепта</param>
    /// <param name="request">Описание добавляемого ингридиента</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPost("{recipeId}/ingredients")]
    [Authorize]
    public async Task<CompositionFieldResponse> AddCompositionField([FromRoute]int recipeId, [FromBody] AddCompositionFieldRequest request)
    {
        var model = mapper.Map<AddCompositionFieldModel>(request);
        model.RecipeId = recipeId;
        model.RequestAuthor = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        var compositionField = await recipeService.AddCompositionField(model);
        var response = mapper.Map<CompositionFieldResponse>(compositionField);

        return response;
    }

    /// <summary>
    /// Изменение ингридиента в существующем рецепту по ID рецепта
    /// </summary>
    /// <param name="compositionFieldId">ID рецепта</param>
    /// <param name="request">Описание обновляемного ингридиента</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpPut("{recipeId}/ingredients/{compositionFieldId}")]
    [Authorize]
    public async Task<CompositionFieldResponse> UpdateCompositionField([FromRoute] int compositionFieldId, [FromBody] UpdateCompositionFieldRequest request)
    {
        var model = mapper.Map<UpdateCompositionFieldModel>(request);
        model.CompositionFieldId = compositionFieldId;
        model.RequestAuthor = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        var compositionField = await recipeService.UpdateCompositionField(model);
        var response = mapper.Map<CompositionFieldResponse>(compositionField);

        return response;
    }

    /// <summary>
    /// Удаление ингридиента в существующем рецепту по ID ингридиента
    /// </summary>
    /// <param name="compositionFieldId">ID ингридиента</param>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesWrite)]
    [HttpDelete("{recipeId}/ingredients/{compositionFieldId}")]
    [Authorize]
    public async Task<IActionResult> DeleteCompositionField([FromRoute] int compositionFieldId)
    {
        var requestAuthor = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        await recipeService.DeleteCompositionField(compositionFieldId, requestAuthor);

        return Ok();
    }
    #endregion



    #region -------------------------------------------------Addition--------------------------------------------------
    /// <summary>
    /// Выдача списка категорий и их ID
    /// </summary>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesRead)]
    [HttpGet("categories")]
    public async Task<IEnumerable<CategoryResponse>> GetCategories()
    {
        var categories = await recipeService.GetCategories();
        return mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    /// <summary>
    /// Выдача списка интгридентов и их ID
    /// </summary>
    /// <returns></returns>
    [RequiredScope(AppScopes.RecipesRead)]
    [HttpGet("ingredients")]
    public async Task<IEnumerable<IngredientResponse>> GetIngredients()
    {
        var ingredients = await recipeService.GetIngredients();
        return mapper.Map<IEnumerable<IngredientResponse>>(ingredients);
    }
    #endregion


    #region ----------------------------------------------Subscriptions------------------------------------------------
    /// <summary>
    /// Подписка на новые комментарии к рецепту
    /// </summary>
    /// <param name="recipeId">ID рецепта</param>
    /// <returns></returns>
    [HttpPost("{recipeId}/subscribe")]    //мы нажимаем на кнопку подписаться на странице автора
    public async Task<SubscriptionToCommentsResponse> AddSubscriptionToComments([FromRoute] int recipeId)
    {
        var subscriber = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);

        AddSubscriptionToCommentsModel model = new AddSubscriptionToCommentsModel()
        {
            SubscriberId = subscriber,
            RecipeId = recipeId
        };  //формы для подписки нет, есть просто кнопка подписаться, а создавать DTO request-а не за чем, поэтому формируем сразу DTO модели

        var response = mapper.Map<SubscriptionToCommentsResponse>(await userAccountService.AddSubscriptionToComments(model));

        return response;
    }

    /// <summary>
    /// Отписка от новых комментариев в рецепте
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <returns></returns>
    [HttpDelete("unsubscribe/{subscriptionId}")]
    [Authorize]
    public async Task<IActionResult> DeleteSubscriptionToComments([FromRoute] int subscriptionId)
    {
        var model = new DeleteSubscriptionModel()
        {
            Subscriber = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value),
            SubscriptionId = subscriptionId
        };

        await userAccountService.DeleteSubscriptionToComments(model);
        return Ok();
    }

    /// <summary>
    /// Подписка на новые рецепты из категории рецептов
    /// </summary>
    /// <param name="categoryId">ID категории</param>
    /// <returns></returns>
    [HttpPost("categories/{categoryId}/subscribe")]    //мы нажимаем на кнопку подписаться на странице автора
    public async Task<SubscriptionToCategoryResponse> AddSubscriptionToCategory([FromRoute] int categoryId)
    {
        var subscriber = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);

        AddSubscriptionToCategoryModel model = new AddSubscriptionToCategoryModel()
        {
            SubscriberId = subscriber,
            CategoryId = categoryId
        };  //формы для подписки нет, есть просто кнопка подписаться, а создавать DTO request-а не за чем, поэтому формируем сразу DTO модели

        var response = mapper.Map<SubscriptionToCategoryResponse>(await userAccountService.AddSubscriptionToCategory(model));

        return response;
    }

    /// <summary>
    /// Отписака от новых рецептов в категории
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <returns></returns>
    ///     Я передаю Id подписки, вместо того, чтобы передавть Id категории для того, чтобы БД не нужно было искать подписку с конкретным подписчиком и категорией
    ///     Вместо этого БД просто найдет конкретную подписку по Id и просто проверит подписчика уже в этой подписке
    ///     По факту я здесть я просто жертвую красотой URL ради производительности и вместо HttpDelete("categories/{categoryId}/subscribe") ставлю другой адрес, но
    ///     при этом БД не придется каждый раз искать подписку. Немного оптимизации, так сказать
    [HttpDelete("categories/unsubscribe/{subscriptionId}")]
    [Authorize]
    public async Task<IActionResult> DeleteSubscriptionToCategory([FromRoute] int subscriptionId)
    {
        var model = new DeleteSubscriptionModel()
        {
            Subscriber = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value),
            SubscriptionId = subscriptionId
        };

        await userAccountService.DeleteSubscriptionToCategory(model);
        return Ok();
    }

    #endregion
}