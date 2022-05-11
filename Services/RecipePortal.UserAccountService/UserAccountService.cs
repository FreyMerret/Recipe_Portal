namespace RecipePortal.UserAccountService;

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipePortal.Common.Exeptions;
using RecipePortal.Common.Validator;
using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Entities;
using RecipePortal.RabbitMqService;
using RecipePortal.Settings;
using RecipePortal.UserAccountService.Models;
using System.Net;
using System.Text;

public class UserAccountService : IUserAccountService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly IRabbitMqTask rabbitMqTask;
    private readonly IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator;
    private readonly IApiSettings apiSettings;

    public UserAccountService(IDbContextFactory<MainDbContext> contextFactory,
                              IMapper mapper,
                              UserManager<User> userManager,
                              IRabbitMqTask rabbitMqTask,
                              IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator,
                              IApiSettings apiSettings)
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.userManager = userManager;
        this.rabbitMqTask = rabbitMqTask;
        this.registerUserAccountModelValidator = registerUserAccountModelValidator;
        this.apiSettings = apiSettings;
    }

    public async Task<IEnumerable<UserAccountModel>> GetUsers(string authorNickname, int offset, int limit)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var users = userManager.Users;
        if (authorNickname != "")
            users = users.Where(x => x.UserName.Contains(authorNickname));

        users = users
            .Skip(offset)
            .Take(limit);

        var data = (await users.ToListAsync()).Select(user => mapper.Map<UserAccountModel>(user)).ToList();

        return data;
    }

    public async Task<UserAccountModel> GetUser(string authorNickname)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var user = userManager.Users.FirstOrDefault(x => x.UserName.Equals(authorNickname))
            ?? throw new ProcessException($"The user was not found");

        var data = mapper.Map<UserAccountModel>(user);

        return data;
    }

    public async Task<UserAccountModel> Create(RegisterUserAccountModel model)
    {
        registerUserAccountModelValidator.Check(model);

        // Find user by email
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
            throw new ProcessException($"User account with email {model.Email} already exist.");

        // Create user account
        user = new User()
        {
            Status = UserStatus.Active,
            Name = model.Name,
            Surname = model.Surname,
            UserName = model.Username,  // Это логин
            Email = model.Email,
            EmailConfirmed = false,
            PhoneNumber = null,
            PhoneNumberConfirmed = false
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new ProcessException($"Creating user account is wrong. {String.Join(", ", result.Errors.Select(s => s.Description))}");

        var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var convertedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(emailConfirmToken)); //переводим токен подтверждения почты в формат url
        await rabbitMqTask.SendEmail(new EmailModel()
        {
            Email = model.Email,
            Subject = "Recipe Portal",
            Message = $"You are registered. Confirm your email: http://localhost:20003/email_confirmation?userEmail={user.Email}&emailConfirmToken={convertedToken}"
        });

        return mapper.Map<UserAccountModel>(user);        
    }

    public async Task EmailConfirmation(string userEmail, string emailConfirmToken)
    {
        var user = await userManager.FindByEmailAsync(userEmail);
        await userManager.ConfirmEmailAsync(user, emailConfirmToken);
    }

    // .. Также здесь можно разместить методы для изменения данных учетной записи,
    // восстановления и смены пароля, подтверждения электронной почты, установки телефона и его подтверждения и т.д.

    public async Task<SubscriptionToAuthorModel> AddSubscriptionToAuthor(Guid subscriber, string authorNickname)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        AddSubscriptionToAuthorModel model = new AddSubscriptionToAuthorModel()
        {
            SubscriberId = subscriber,
            //AuthorId = userManager.FindByNameAsync(authorNickname).Result.Id
            AuthorId = context.Users.FirstOrDefault(x => x.UserName.Equals(authorNickname)).Id
        };

        var subscription = mapper.Map<SubscriptionToAuthor>(model);

        await context.SubscriptionsToAuthor.AddAsync(subscription);
        context.SaveChanges();

        var response = mapper.Map<SubscriptionToAuthorModel>(context.SubscriptionsToAuthor.Include(x => x.Author).First(x=>x.Id.Equals(subscription.Id)));
        return response;
    }

    public async Task DeleteSubscriptionToAuthor(DeleteSubscriptionModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var subscription = await context.SubscriptionsToAuthor.FirstOrDefaultAsync(x => x.Id.Equals(model.SubscriptionId))
            ?? throw new ProcessException($"The subscription does not exist");

        if (subscription.SubscriberId == model.Subscriber)   //проверка, что удаляется своя подписка, а не чужая
        {
            context.SubscriptionsToAuthor.Remove(subscription);
            context.SaveChanges();
        }
        else
            throw new ProcessException($"The subscription isn't yours");
    }

    public async Task<SubscriptionToCategoryModel> AddSubscriptionToCategory(AddSubscriptionToCategoryModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var subscription = mapper.Map<SubscriptionToCategory>(model);

        await context.SubscriptionsToCategory.AddAsync(subscription);
        context.SaveChanges();

        var response = mapper.Map<SubscriptionToCategoryModel>(context.SubscriptionsToCategory.Include(x => x.Category).First(x => x.Id.Equals(subscription.Id)));
        return response;
    }

    public async Task DeleteSubscriptionToCategory(DeleteSubscriptionModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var subscription = await context.SubscriptionsToCategory.FirstOrDefaultAsync(x => x.Id.Equals(model.SubscriptionId))
            ?? throw new ProcessException($"The subscription does not exist");

        if (subscription.SubscriberId == model.Subscriber)   //проверка, что удаляется своя подписка, а не чужая
        {
            context.SubscriptionsToCategory.Remove(subscription);
            context.SaveChanges();
        }
        else
            throw new ProcessException($"The subscription isn't yours");
    }

    public async Task<SubscriptionToCommentsModel> AddSubscriptionToComments(AddSubscriptionToCommentsModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var subscription = mapper.Map<SubscriptionToComments>(model);

        await context.SubscriptionsToComments.AddAsync(subscription);
        context.SaveChanges();

        var response = mapper.Map<SubscriptionToCommentsModel>(context.SubscriptionsToComments.Include(x => x.Recipe).First(x => x.Id.Equals(subscription.Id)));
        return response;
    }

    public async Task DeleteSubscriptionToComments(DeleteSubscriptionModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var subscription = await context.SubscriptionsToComments.FirstOrDefaultAsync(x => x.Id.Equals(model.SubscriptionId))
            ?? throw new ProcessException($"The subscription does not exist");

        if (subscription.SubscriberId == model.Subscriber)   //проверка, что удаляется своя подписка, а не чужая
        {
            context.SubscriptionsToComments.Remove(subscription);
            context.SaveChanges();
        }
        else
            throw new ProcessException($"The subscription isn't yours");
    }

    public async Task<AllSubscriptionsModel> GetSubscriptions(Guid user)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        AllSubscriptionsModel allSubscriptions = new AllSubscriptionsModel();

        var subToAuthors = context
            .SubscriptionsToAuthor
            .Where(x => x.SubscriberId.Equals(user))
            .Include(x => x.Author);

        var subToCategories = (context
            .SubscriptionsToCategory
            .Where(x => x.SubscriberId.Equals(user))
            .Include(x => x.Category));

        var subToComments = (context
            .SubscriptionsToComments
            .Where(x => x.SubscriberId.Equals(user))
            .Include(x => x.Recipe).ThenInclude(x => x.CompositionFields).ThenInclude(x => x.Ingredient)
            .Include(x => x.Recipe).ThenInclude(x => x.Author)
            .Include(x => x.Recipe).ThenInclude(x => x.Category));

        allSubscriptions.SubscriptionsToAuthors = (await subToAuthors.ToListAsync()).Select(subToAuthors => mapper.Map<SubscriptionToAuthorModel>(subToAuthors)).ToList();
        allSubscriptions.SubscriptionsToCategories = (await subToCategories.ToListAsync()).Select(subToCategories => mapper.Map<SubscriptionToCategoryModel>(subToCategories)).ToList();
        allSubscriptions.SubscriptionsToComments = (await subToComments.ToListAsync()).Select(subToComments => mapper.Map<SubscriptionToCommentsModel>(subToComments)).ToList();

        return allSubscriptions;
    }

}
