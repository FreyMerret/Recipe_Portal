using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web.Resource;
using RecipePortal.Common.Security;
using Microsoft.AspNetCore.Mvc;
using RecipePortal.API.Controllers.UserAccounts.Models;
using RecipePortal.UserAccountService;
using RecipePortal.UserAccountService.Models;

namespace RecipePortal.API.Controllers.UserAccounts;

[Route("api/v{version:apiVersion}/accounts")]
[ApiController]
[ApiVersion("1.0")]
public class UserAccountsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<UserAccountsController> logger;
    private readonly IUserAccountService userAccountService;

    public UserAccountsController(IMapper mapper, ILogger<UserAccountsController> logger, IUserAccountService userAccountService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.userAccountService = userAccountService;
    }

    /// <summary>
    /// Выдача всех пользователей
    /// </summary>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<IEnumerable<UserAccountResponse>> GetUsers([FromQuery] string authorNickname = "", [FromQuery] int offset = 0, [FromQuery] int limit = 20)
    {
        var users = await userAccountService.GetUsers(authorNickname, offset, limit);

        var response = mapper.Map<IEnumerable<UserAccountResponse>>(users);

        return response;
    }

    /// <summary>
    /// Выдача пользователя с его рецептами
    /// </summary>
    /// <param name="authorNickname"></param>
    /// <returns></returns>
    [HttpGet("{authorNickname}")]
    public async Task<UserAccountResponse> GetUser([FromRoute] string authorNickname)
    {
        var user = await userAccountService.GetUser(authorNickname);

        var response = mapper.Map<UserAccountResponse>(user);

        return response;
    }

    /// <summary>
    /// Создание нового пользователя
    /// </summary>
    /// <param name="request">Создание нового пользователя</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<UserAccountResponse> Register([FromBody] RegisterUserAccountRequest request)
    {
        var user = await userAccountService.Create(mapper.Map<RegisterUserAccountModel>(request));

        var response = mapper.Map<UserAccountResponse>(user);

        return response;
    }

    //[HttpPut("/change_password")]
    //public async Task<IActionResult> ChangePassword([FromBody] RegisterUserAccountRequest request)
    //{
    //    await userAccountService.ChangePassword(mapper.Map<RegisterUserAccountModel>(request));       

    //    return Ok();
    //}

    #region -----------------------------------------------Subscriptions------------------------------------------------

    /// <summary>
    /// Подписка на новые рецепты другого автора
    /// </summary>
    /// <param name="authorNickname">Ник автора, на которого нужно подписаться</param>
    /// <returns></returns>
    [HttpPost("{authorNickname}/subscription")]    //мы нажимаем на кнопку подписаться на странице автора
    [Authorize]
    public async Task<SubscriptionToAuthorResponse> AddSubscriptionToAuthor([FromRoute] string authorNickname)
    {
        var subscriber = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);

        var response = mapper.Map<SubscriptionToAuthorResponse>(await userAccountService.AddSubscriptionToAuthor(subscriber, authorNickname));   
                                                      //чтобы не лезть ни в БД, ни к UserManager мы просто отдаем все инфу контроллеру без DTO
                                                      //формы для подписки нет, есть просто кнопка подписаться, а создавать DTO request-а не за чем
        return response;
    }

    /// <summary>
    /// Отписка от автора
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <returns></returns>
    [HttpDelete("unsubscribe/{subscriptionId}")]    //мы нажимаем на кнопку отписаться на странице автора
    [Authorize]
    public async Task<IActionResult> DeleteSubscriptionToAuthor([FromRoute] int subscriptionId)
    {
        var model = new DeleteSubscriptionModel()
        {
            Subscriber = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value),
            SubscriptionId = subscriptionId
        };

        await userAccountService.DeleteSubscriptionToAuthor(model);
        return Ok();
    }

    /// <summary>
    /// Выдача всех подписок текущего пользователя
    /// </summary>
    /// <returns></returns>
    [HttpGet("my_subscriptions")]
    [Authorize]
    public async Task<AllSubscriptionsResponse> GetSubscriptions()
    {
        var user = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);

        var response = mapper.Map<AllSubscriptionsResponse>(await userAccountService.GetSubscriptions(user));

        return response;
    }

    #endregion
}
