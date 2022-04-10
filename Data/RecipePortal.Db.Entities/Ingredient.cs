namespace RecipePortal.Db.Entities;

public class Ingredient : BaseEntity
{
    public string Name { get; set; }

    public virtual ICollection<Composition>? Composition { get; set; }  //для того, чтобы знать в каких рецептах есть этот ингридиент

    public virtual ICollection<CompositionIngredient>? CompositionIngredients { get; set; } //для связи многи-ко-многим с таблицей "Composition" (состав)
}
