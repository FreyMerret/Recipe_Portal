namespace RecipePortal.Db.Entities;

public class Composition : BaseEntity   //состав рецепта
{
    //одна строка в таблице содержит один ингридиент и его количество. Рецепт имеет несколько таких строк. Многие к одному
    public int IngredientId { get; set; }

    public virtual Ingredient Ingredient { get; set; }

    public int? RecipeId { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public string Quantity { get; set; }    //количество ингридиента

    public virtual ICollection<CompositionIngredient> CompositionIngredients { get; set; } //промежуточная таблица для связи с ингридтентами многие-ко-многим

}
