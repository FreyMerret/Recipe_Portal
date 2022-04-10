namespace RecipePortal.Db.Entities;

public class CompositionIngredient : BaseEntity
{
    public int CompositionId { get; set; }

    public virtual Composition Composition { get; set; }

    public int IngredientId { get; set; }

    public virtual Ingredient Ingredient { get; set; }
}
