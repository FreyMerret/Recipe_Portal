namespace RecipePortal.Db.Entities;

public class Recipe : BaseEntity
{
    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string LongDescription { get; set; }

    public virtual ICollection<Composition>  Composition { get; set; }  //список ингридиентов и их количество

    public Guid AuthorId { get; set; }

    public virtual User Author { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<Comment>? Comments { get; set; }
}
