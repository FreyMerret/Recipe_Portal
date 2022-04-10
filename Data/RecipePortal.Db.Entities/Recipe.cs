namespace RecipePortal.Db.Entities;

public class Recipe : BaseEntity
{
    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; }

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }

    public virtual ICollection<RecipeCompositionField> RecipeCompositionFields { get; set; }  //список ингридиентов и их количество

    public virtual ICollection<Comment>? Comments { get; set; }
}
