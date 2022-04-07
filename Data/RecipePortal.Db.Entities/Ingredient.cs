namespace RecipePortal.Db.Entities;

public class Ingredient : BaseEntity
{
    public string Name { get; set; }

    public virtual ICollection<Recipe> Resipes { get; set; }

}
