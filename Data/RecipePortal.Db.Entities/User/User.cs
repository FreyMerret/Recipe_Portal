namespace RecipePortal.Db.Entities;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }

    public string Surname { get; set; }

    public UserStatus Status { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }

    public virtual ICollection<SubscriptionToAuthor> SubscriptionsToAuthor { get; set; }

    public virtual ICollection<SubscriptionToAuthor> Subscribers { get; set; }

    public virtual ICollection<SubscriptionToCategory> SubscriptionsToCategory { get; set; }

    public virtual ICollection<SubscriptionToComments> SubscriptionsToComments { get; set; }


}
