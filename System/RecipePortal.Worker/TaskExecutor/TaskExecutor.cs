using Microsoft.EntityFrameworkCore;
using RecipePortal.Db.Context.Context;
using RecipePortal.Db.Entities;
using RecipePortal.EmailService;
using RecipePortal.RabbitMqService;

namespace RecipePortal.Worker;

public class TaskExecutor : ITaskExecutor
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly ILogger<TaskExecutor> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly IRabbitMq rabbitMq;

    public TaskExecutor(
        IDbContextFactory<MainDbContext> contextFactory,
        ILogger<TaskExecutor> logger,
        IServiceProvider serviceProvider,
        IRabbitMq rabbitMq
    )
    {
        this.contextFactory = contextFactory;
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.rabbitMq = rabbitMq;
    }

    private async Task Execute<T>(Func<T, Task> action)
    {//Func Инкапсулирует метод с одним параметром, который возвращает значение типа, указанного в параметре TResult
        try
        {
            using var scope = serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetService<T>();
            if (service != null)
                await action(service);
            else
                logger.LogError($"Error: {action.ToString()} wasn`t resolved");
        }
        catch (Exception e)
        {
            logger.LogError($"Error: {RabbitMqTaskQueueNames.SEND_EMAIL}: {e.Message}");
            throw;
        }
    }

    public void Start()
    {
        rabbitMq.Subscribe<EmailModel>(RabbitMqTaskQueueNames.SEND_EMAIL, async data    //data - делегат
            => await Execute<IEmailSender>(async service => //Func в Execute икапсулирует метод service - логирование отправки и сама отправка и лог в случае ошибки отправки
            {
                logger.LogDebug($"{RabbitMqTaskQueueNames.SEND_EMAIL}: {data.Email} {data.Message}");
                await service.SendEmailAsync(data);
            }));

        rabbitMq.Subscribe<int>(RabbitMqTaskQueueNames.MAILING_NEW_RECIPE, async data    //data - делегат
            => await Execute<IEmailSender>(async service => //Func в Execute икапсулирует метод service - логирование отправки и сама отправка и лог в случае ошибки отправки
            {
                logger.LogDebug($"{RabbitMqTaskQueueNames.MAILING_NEW_RECIPE}: the formation of a mailing list about a new recipe (id: {data})has begun");

                using var context = await contextFactory.CreateDbContextAsync();

                var recipe = await context.Recipes.FirstOrDefaultAsync(r => r.Id.Equals(data));
                int category = recipe.CategoryId;
                Guid author = recipe.AuthorId;

                //делаем выборку адресов подписавшихся на категорию и на автора, соединяем их и убираем повторы
                var mailingList = (await context
                    .SubscriptionsToCategory
                    .Include(i => i.Subscriber)
                    .Where(w => w.CategoryId.Equals(category) || w.CategoryId.Equals(0))    //0 - подписка на все категории (все новые рецепты) //пока без нее)))
                    .Select(s => s.Subscriber.Email)
                    .ToListAsync())
                .Union(
                    await context
                    .SubscriptionsToAuthor
                    .Include(i => i.Subscriber)
                    .Where(w => w.AuthorId.Equals(author))
                    .Select(s => s.Subscriber.Email)
                    .ToListAsync())
                .Distinct();

                foreach (var email in mailingList)
                {
                    await service.SendEmailAsync(email, "New recipe in Recipe Portal", $"You can find new recipe at http://localhost:20003/recipes/{recipe.Id}");
                };

            }));

        rabbitMq.Subscribe<int>(RabbitMqTaskQueueNames.MAILING_NEW_COMMENT, async data
            => await Execute<IEmailSender>(async service =>
            {
                logger.LogDebug($"{RabbitMqTaskQueueNames.MAILING_NEW_RECIPE}: the formation of a mailing list about a new comment (id: {data})has begun");

                using var context = await contextFactory.CreateDbContextAsync();

                var comment = await context.Comments.FirstOrDefaultAsync(r => r.Id.Equals(data));
                int recipeId = comment.RecipeId;


                    var mailingList = (await context
                        .SubscriptionsToComments
                        .Include(i => i.Subscriber)
                        .Where(w => w.RecipeId.Equals(recipeId))
                        .Select(s => s.Subscriber.Email)
                        .ToListAsync());

                foreach (var email in mailingList)
                {
                    await service.SendEmailAsync(email, "New comment added", $"You can find new comment at http://localhost:20003/recipes/{recipeId}");
                };

            }));
    }
}
