namespace Conduit.Images.Domain.Images.Common;

public class ArticleImageDomainModel : ImageDomainModel
{
    public ArticleImageDomainModel(
        Guid id,
        Guid userId,
        string url,
        string storageName,
        string mediaType,
        DateTime uploaded,
        Guid? articleId = null) : base(id, userId, url, storageName, mediaType, uploaded)
    {
        ArticleId = articleId;
    }

    /// <summary>
    ///     ArticleId
    /// </summary>
    /// <value>Null if not yet assigned</value>
    public Guid? ArticleId { get; }
}
