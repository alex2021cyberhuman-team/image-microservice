public class ArticleImageResponseModel : ImageResponseModel
{
    public ArticleImageResponseModel(
        Guid? articleId,
        Guid id,
        string url,
        string mediaType) : base(id, url, mediaType)
    {
        ArticleId = articleId;
    }

    public Guid? ArticleId { get; }
}
