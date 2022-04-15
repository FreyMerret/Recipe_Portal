namespace RecipePortal.Db.Entities;

public class Ingredient : BaseEntity
{
    public string Name { get; set; }

    public virtual ICollection<CompositionField> RecipeCompositionFields { get; set; }  //для того, чтобы знать в каких рецептах есть этот ингридиент
}
