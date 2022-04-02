using Conduit.Images.Domain.Images.Common;

namespace Conduit.Images.DataAccess.Images.Models;

public class ArticleImageDbModel
{

    public ArticleImageDbModel(
            Guid id,
            Guid userId,
            string storageName,
            string mediaType,
            DateTime uploaded,
            Guid? articleId = null)
    {
        Id = id;
        UserId = userId;
        StorageName = storageName;
        MediaType = mediaType;
        Uploaded = uploaded;
        ArticleId = articleId;
    }
    public Guid Id { get; }

    public Guid UserId { get; }

    public string StorageName { get; }

    public string MediaType { get; }

    public DateTime Uploaded { get; }

    public Guid? ArticleId { get; }

    public ArticleImageDomainModel ToDomainModel(string url = "") => new(
            Id,
            UserId,
            url,
            StorageName,
            MediaType,
            Uploaded,
            ArticleId
        );
}