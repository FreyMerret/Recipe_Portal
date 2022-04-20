namespace RecipePortal.Db.Entities;

public class SubscriptionToAuthor : BaseEntity
{
    public Guid SubscriberId { get; set; }
    public virtual User Subscriber { get; set; }
    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; }
}
