namespace RecipePortal.Db.Entities;

public class Category : BaseEntity

{
    public string Title;

    public virtual ICollection<Recipe> Resipes { get; set; }
}
