namespace RecipePortal.Db.Entities;

public class RecipeCompositionField : BaseEntity   //состав рецепта
{
    public int? RecipeId { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public int IngredientId { get; set; }

    public virtual Ingredient Ingredient { get; set; }

    public string Quantity { get; set; }    //количество ингридиента
}
