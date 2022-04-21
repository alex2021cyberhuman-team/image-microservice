namespace Conduit.Images.Domain.Images.GetArticleImages;

public class GetArticleImagesRequest
{
    public GetArticleImagesRequest(Guid userId, Guid articleId)
    {
        UserId = userId;
        ArticleId = articleId;
    }

    public Guid ArticleId { get; set; }
    
    public Guid UserId { get; set; }
}
