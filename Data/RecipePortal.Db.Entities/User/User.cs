namespace RecipePortal.Db.Entities;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }

    public string Surname { get; set; }

    public UserStatus Status { get; set; }
}
