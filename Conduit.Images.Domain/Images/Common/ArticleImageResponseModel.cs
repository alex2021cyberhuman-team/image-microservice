namespace Conduit.Images.Domain.Images.Common;

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

    public static implicit operator ArticleImageResponseModel(ArticleImageDomainModel image) => new(
        image.ArticleId,
        image.Id,
        image.Url,
        image.MediaType);
}
