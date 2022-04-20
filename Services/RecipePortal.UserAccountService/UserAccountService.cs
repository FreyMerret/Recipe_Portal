    namespace RecipePortal.UserAccountService;

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipePortal.Common.Exeptions;
using RecipePortal.Common.Validator;
using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Entities;
using RecipePortal.RabbitMqService;
using RecipePortal.UserAccountService.Models;

public class UserAccountService : IUserAccountService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly IRabbitMqTask rabbitMqTask;
    private readonly IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator;

    public UserAccountService(IDbContextFactory<MainDbContext> contextFactory, IMapper mapper, UserManager<User> userManager, IRabbitMqTask rabbitMqTask, IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator)
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.userManager = userManager;
        this.rabbitMqTask = rabbitMqTask;
        this.registerUserAccountModelValidator = registerUserAccountModelValidator;
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
            UserName = model.Username,  // Ёто логин
            Email = model.Email,
            EmailConfirmed = true, // “ак как это учебный проект, то сразу считаем, что почта подтверждена. ¬ реальном проекте, скорее всего, надо будет ее подтвердить через ссылку в письме
            PhoneNumber = null,
            PhoneNumberConfirmed = false
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new ProcessException($"Creating user account is wrong. {String.Join(", ", result.Errors.Select(s => s.Description))}");


        //await rabbitMqTask.SendEmail(new EmailModel()
        //{
        //    Email = model.Email,
        //    Subject = "Recipe Portal",
        //    Message = "Your account was registered successful"
        //}); 

        return mapper.Map<UserAccountModel>(user);
    }

    // .. “акже здесь можно разместить методы дл€ изменени€ данных учетной записи,
    // восстановлени€ и смены парол€, подтверждени€ электронной почты, установки телефона и его подтверждени€ и т.д.

    public async Task AddSubscriptionToAuthor(Guid subscriber, string autorNickname)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        SubscriptionToAuthorModel model = new SubscriptionToAuthorModel()
        {
            SubscriberId = subscriber,
            AuthorId = userManager.FindByNameAsync(autorNickname).Result.Id
        };

        var subscription = mapper.Map<SubscriptionToAuthor>(model);

        await context.SubscriptionsToAuthor.AddAsync(subscription);
        context.SaveChanges();
    }

    public async Task AddSubscriptionToCategory(SubscriptionToCategoryModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var subscription = mapper.Map<SubscriptionToCategory>(model);

        await context.SubscriptionsToCategory.AddAsync(subscription);
        context.SaveChanges();
    }

    public async Task AddSubscriptionToComments(SubscriptionToCommentsModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        
        var subscription = mapper.Map<SubscriptionToComments>(model);

        await context.SubscriptionsToComments.AddAsync(subscription);
        context.SaveChanges();
    }



}
