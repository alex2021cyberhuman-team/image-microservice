namespace Conduit.Images.Domain.Comments.Delete;

public class CommentDeleteRequest
{
    public Guid CommentId { get; set; }

    public string ArticleSlug { get; set; } = string.Empty;

    public Guid AuthorId { get; set; }
}
