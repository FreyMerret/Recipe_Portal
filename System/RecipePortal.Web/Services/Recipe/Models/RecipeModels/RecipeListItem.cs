namespace RecipePortal.Web;

public class RecipeListItem
{
    public int RecipeId { get; set; }

    public string Author { get; set; }

    public int CategoryId { get; set; }
    public string Category { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Text { get; set; }

    //поля состава рецепта
    public virtual List<CompositionFieldItem> CompositionFields { get; set; }
}
