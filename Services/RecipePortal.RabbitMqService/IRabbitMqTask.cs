using System.Threading.Tasks;

namespace RecipePortal.RabbitMqService;

public interface IRabbitMqTask
{
    Task SendEmail(EmailModel email);

    Task MailingNewRecipe(int recipeId);

    Task MailingNewComment(int commentId);
}
