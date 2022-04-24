using Dapper.FluentMap.Mapping;

namespace Conduit.Images.DataAccess.Articles;

public class ArticleDbModel
{
    public Guid Id { get; set; }

    public string Slug { get; set; } = string.Empty;

    public Guid AuthorId { get; set; }

    public class EntityMap : EntityMap<ArticleDbModel>
    {
        public EntityMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Slug).ToColumn("slug");
            Map(p => p.AuthorId).ToColumn("author_id");
        }
    }
}
