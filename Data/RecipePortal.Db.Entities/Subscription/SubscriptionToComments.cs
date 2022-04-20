namespace RecipePortal.Db.Entities;

public class SubscriptionToComments : BaseEntity
{
    public Guid SubscriberId { get; set; }
    public virtual User Subscriber { get; set; }
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }
}
