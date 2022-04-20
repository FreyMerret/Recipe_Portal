using RecipePortal.RabbitMqService;

namespace RecipePortal.EmailService;

public interface IEmailSender
{
    Task SendEmailAsync(EmailModel model);
    Task SendEmailAsync(string email, string subject, string message);
}
