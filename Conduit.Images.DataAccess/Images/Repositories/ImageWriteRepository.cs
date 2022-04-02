using Conduit.Images.DataAccess.Extensions;
using Conduit.Images.DataAccess.Images.Models;
using Conduit.Images.Domain.Images.Common;
using Conduit.Images.Domain.Images.Services.Repositories;
using Dapper;
using static Conduit.Images.DataAccess.Extensions.TaskExtensions;

namespace Conduit.Images.DataAccess.Images.Repositories;

public class ImageWriteRepository : IImageWriteRepository
{
    private readonly IImageStorage _imageStorage;
    private readonly ConnectionProvider _connectionProvider;

    public ImageWriteRepository(
        IImageStorage imageStorage,
        ConnectionProvider connectionProvider)
    {
        _imageStorage = imageStorage;
        _connectionProvider = connectionProvider;
    }

    public async Task RemoveAsync(ArticleImageDomainModel articleImageDomainModel, CancellationToken cancellationToken = default)
    {
        const string removeArticleImageCommand = @"DELETE FROM ""article_image""
        WHERE ""id"" = @Id";
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        await connection.ExecuteAsync(removeArticleImageCommand, new { articleImageDomainModel.Id }).SingleResult();
        await _imageStorage.RemoveAsync(articleImageDomainModel.StorageName, cancellationToken);
    }

    public async Task<ArticleImageDomainModel> SaveAsync(
        Guid userId,
        Stream stream,
        string mediaType,
        Guid imageId,
        string storageName,
        string imageUrl,
        DateTime uploaded,
        CancellationToken cancellationToken = default)
    {
        await _imageStorage.StoreAsync(storageName, stream, cancellationToken);
        var dbModel = await InsertAsync(imageId, userId, storageName, mediaType, uploaded, cancellationToken);
        return dbModel.ToDomainModel(imageUrl);
    }

    private async Task<ArticleImageDbModel> InsertAsync(Guid imageId, Guid userId, string storageName, string mediaType, DateTime uploaded, CancellationToken cancellationToken)
    {
        var dbModel = new ArticleImageDbModel(imageId, userId, storageName, mediaType, uploaded);
        var connection = await _connectionProvider.ProvideAsync(cancellationToken);
        const string insertArticleImageCommand = @"INSERT INTO ""article_image"" (
            ""id"",
            ""user_id"",
            ""storage_name"",
            ""media_type"",
            ""uploaded""
        ) VALUES (
            @Id,
            @UserId,
            @StorageName,
            @MediaType,
            @Uploaded,
            @ArticleId
        );";
        await connection.ExecuteAsync(insertArticleImageCommand, dbModel).SingleResult();
        return dbModel;
    }
}

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
        var dbModel = await connection.QueryFirstOrDefaultAsync(findArticleImageQuery, new { Id = id });
        var domainModel = dbModel.ToDomainModel(_imageUrlProvider.Provide(dbModel.StorageName));
        return domainModel;
    }
}