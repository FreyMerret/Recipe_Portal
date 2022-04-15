namespace RecipePortal.Db.Entities;

public class Comment : BaseEntity
{
    //править на User

    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; }
    public string CommentText { get; set; }
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }
}
