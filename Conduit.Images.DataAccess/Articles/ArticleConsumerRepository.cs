using Conduit.Images.Domain.Articles;
using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;
using Dapper;
using Conduit.Images.DataAccess.Extensions;

namespace Conduit.Images.DataAccess.Articles;

public class ArticleConsumerRepository : IArticleConsumerRepository
{
    private readonly ConnectionProvider _connectionProvider;

    public ArticleConsumerRepository(ConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task CreateAsync(CreateArticleEventModel eventModel)
    {
        var connection = await _connectionProvider.ProvideAsync();
        const string createArticleQuery = @"INSERT INTO ""article"" (
            ""id"",
            ""slug"",
            ""author_id""
        ) VALUES (
            @Id,
            @Slug,
            @AuthorId
        );";
        await connection.ExecuteAsync(createArticleQuery, new
        {
            Id = eventModel.Id,
            Slug = eventModel.Slug,
            AuthorId = eventModel.AuthorId
        });
    }

    public async Task RemoveAsync(DeleteArticleEventModel eventModel)
    {
        var connection = await _connectionProvider.ProvideAsync();
        const string removeArticleQuery = @"DELETE FROM ""article""
        WHERE ""id"" = @Id;";
        await connection.ExecuteAsync(removeArticleQuery, new
        {
            Id = eventModel.Id
        }).SingleResult();
    }

    public async Task UpdateAsync(UpdateArticleEventModel eventModel)
    {
        var connection = await _connectionProvider.ProvideAsync();
        const string updateArticleQuery = @"UPDATE ""article""
        SET
            ""slug"" = @Slug,
            ""author_id"" = @AuthorId
        WHERE 
            ""id"" = @Id;";
        await connection.ExecuteAsync(updateArticleQuery, new
        {
            Slug = eventModel.Slug,
            Id = eventModel.Id,
            AuthorId = eventModel.AuthorId
        }).SingleResult();
    }
}
