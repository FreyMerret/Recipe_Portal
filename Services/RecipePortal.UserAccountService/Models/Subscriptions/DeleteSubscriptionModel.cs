namespace RecipePortal.UserAccountService.Models;

public class DeleteSubscriptionModel
{
    public Guid Subscriber { get; set; }

    public int SubscriptionId { get; set; }
}
