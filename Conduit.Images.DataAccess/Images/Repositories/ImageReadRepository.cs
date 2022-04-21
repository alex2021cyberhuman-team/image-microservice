using System.Data;
using Conduit.Images.DataAccess.Images.Models;
using Conduit.Images.Domain.Images.Common;
using Conduit.Images.Domain.Images.Services.Repositories;
using Dapper;
using Npgsql;

namespace Conduit.Images.DataAccess.Images.Repositories;

public class ImageReadRepository : IImageReadRepository
{
    private readonly ConnectionProvider _connectionProvider;
    private readonly IImageUrlProvider _imageUrlProvider;

    public ImageReadRepository(ConnectionProvider connectionProvider, IImageUrlProvider imageUrlProvider)
    {
        _connectionProvider = connectionProvider;
        _imageUrlProvider = imageUrlProvider;
    }

    public async Task<ArticleImageDomainModel?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        const string findArticleImageQuery = @"SELECT 
            ""id"",
            ""article_id""
            ""user_id"",
            ""storage_name"",
            ""media_type"",
            ""uploaded""
        FROM ""article_image""
        WHERE ""id"" = @Id;";
        var dbModel = await connection.QueryFirstOrDefaultAsync<ArticleImageDbModel>(findArticleImageQuery, new { Id = id });
        var domainModel = dbModel.ToDomainModel(_imageUrlProvider.Provide(dbModel.StorageName));
        return domainModel;
    }

    public async Task<IReadOnlyCollection<ArticleImageDomainModel>> FindByIdsAsync(HashSet<Guid> imageIds, CancellationToken cancellationToken)
    {
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        const string findArticleImageByIdsQuery = @"SELECT 
            ""id"",
            ""article_id""
            ""user_id"",
            ""storage_name"",
            ""media_type"",
            ""uploaded""
        FROM ""article_image""
        WHERE ""id"" IN @Ids;";
        return await QueryAsync(new {Ids = imageIds}, connection, findArticleImageByIdsQuery);
    }

    public async Task<IReadOnlyCollection<ArticleImageDomainModel>> FindByArticleIdAsync(Guid articleId,
        CancellationToken cancellationToken)
    {
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        const string findArticleImagesByArticleId = @"SELECT 
            ""id"",
            ""article_id""
            ""user_id"",
            ""storage_name"",
            ""media_type"",
            ""uploaded""
        FROM ""article_image""
        WHERE ""article_id"" = @ArticleId";
        return await QueryAsync(new {ArticleId = articleId}, connection, findArticleImagesByArticleId);
    }

    private async Task<List<ArticleImageDomainModel>> QueryAsync(object parameters, IDbConnection connection, string sql)
    {
        var dbModels = await connection.QueryAsync<ArticleImageDbModel>(sql, parameters);
        var domainModels = dbModels
            .Select(x => x.ToDomainModel(_imageUrlProvider.Provide(x.StorageName)))
            .ToList();
        return domainModels;
    }
}
