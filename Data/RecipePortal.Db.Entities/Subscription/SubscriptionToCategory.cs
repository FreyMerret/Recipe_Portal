namespace RecipePortal.Db.Entities;

public class SubscriptionToCategory : BaseEntity
{
    public Guid SubscriberId { get; set; }
    public virtual User Subscriber { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
}
