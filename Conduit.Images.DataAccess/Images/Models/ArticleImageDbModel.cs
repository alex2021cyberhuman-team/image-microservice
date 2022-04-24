using Conduit.Images.Domain.Images.Common;
using Dapper.FluentMap.Mapping;

namespace Conduit.Images.DataAccess.Images.Models;

public class ArticleImageDbModel
{
    public ArticleImageDbModel()
    {        
    }

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

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string StorageName { get; set; } = string.Empty;

    public string MediaType { get; set; } = string.Empty;

    public DateTime Uploaded { get; set; }

    public Guid? ArticleId { get; set; }

    public ArticleImageDomainModel ToDomainModel(string url = "") => new(
            Id,
            UserId,
            url,
            StorageName,
            MediaType,
            Uploaded,
            ArticleId
        );

    public class EntityMap : EntityMap<ArticleImageDbModel>
    {
        public EntityMap()
        {
            Map(i => i.Id).ToColumn("id");
            Map(i => i.UserId).ToColumn("user_id");
            Map(i => i.StorageName).ToColumn("storage_name");
            Map(i => i.MediaType).ToColumn("media_type");
            Map(i => i.Uploaded).ToColumn("uploaded");
            Map(i => i.ArticleId).ToColumn("article_id");
        }
    }
}
