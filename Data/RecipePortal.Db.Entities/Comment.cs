namespace RecipePortal.Db.Entities;

public class Comment : BaseEntity
{
    //править на User
    public string Author { get; set; }
    public string CommentText { get; set; }
    public int? Rating { get; set; }
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }
}
