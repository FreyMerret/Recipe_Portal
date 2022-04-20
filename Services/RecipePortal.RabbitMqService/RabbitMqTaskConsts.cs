namespace RecipePortal.RabbitMqService;

public static class RabbitMqTaskQueueNames
{
    public const string SEND_EMAIL = "RECIPE_PORTAL_SEND_EMAIL";
    public const string MAILING_NEW_RECIPE = "RECIPE_PORTAL_MAILING_NEW_RECIPE";
    public const string MAILING_NEW_COMMENT = "RECIPE_PORTAL_MAILING_NEW_COMMENT";
}
