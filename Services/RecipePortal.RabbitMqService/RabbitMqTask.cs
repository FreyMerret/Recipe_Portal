namespace RecipePortal.RabbitMqService;

public class RabbitMqTask : IRabbitMqTask
{
    private readonly IRabbitMq rabbitMq;

    public RabbitMqTask(IRabbitMq rabbitMq)
    {
        this.rabbitMq = rabbitMq;
    }

    public async Task SendEmail(EmailModel email)
    {
        await rabbitMq.PushAsync(RabbitMqTaskQueueNames.SEND_EMAIL, email);
    }

    public async Task MailingNewRecipe(int recipeId)
    {
        await rabbitMq.PushAsync(RabbitMqTaskQueueNames.MAILING_NEW_RECIPE, recipeId);
    }

    public async Task MailingNewComment(int commentId)
    {
        await rabbitMq.PushAsync(RabbitMqTaskQueueNames.MAILING_NEW_COMMENT, commentId);
    }
}
