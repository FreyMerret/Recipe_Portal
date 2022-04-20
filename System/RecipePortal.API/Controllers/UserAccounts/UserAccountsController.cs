using AutoMapper;
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

    /// <summary>
    /// Подписка на новые рецепты другого автора
    /// </summary>
    /// <param name="authorNickname">Ник автора, на которого нужно подписаться</param>
    /// <returns></returns>
    [HttpPost("{authorNickname}/subscribe")]    //мы нажимаем на кнопку подписаться на странице автора
    public async Task<IActionResult> AddSubscriptionToAuthor([FromRoute] string authorNickname)
    {
        var subscriber = new Guid(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);

        await userAccountService.AddSubscriptionToAuthor(subscriber, authorNickname);   //чтобы не лезть ни в БД, ни к UserManager мы просто отдаем все инфу контроллеру без DTO
                                                                                        //формы для подписки нет, есть просто кнопка подписаться, а создавать DTO request-а не за чем
        return Ok();
    }

}
