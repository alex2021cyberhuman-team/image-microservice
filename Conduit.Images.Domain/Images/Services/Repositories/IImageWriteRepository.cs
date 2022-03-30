using Conduit.Images.Domain.Images.Common;

namespace Conduit.Images.Domain.Images.Services.Repositories;

public interface IImageReadRepository
{
    public Task<ArticleImageDomainModel?> FindAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IImageWriteRepository
{
    public Task<ArticleImageDomainModel> SaveAsync(
        Guid userId,
        Stream stream,
        string mediaType,
        Guid imageId,
        string storageName,
        string imageUrl,
        DateTime uploaded,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Removes image from database and its file
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns><code>True</code> if image found and removed, otherwise <code>False</code></returns>
    public Task RemoveAsync(ArticleImageDomainModel articleImageDomainModel, CancellationToken cancellationToken = default);
}

public interface IImageStorage
{
    public Task RemoveAsync(string storageName, CancellationToken cancellationToken);

    public Task StoreAsync(string storageName, Stream stream, CancellationToken cancellationToken);

    public Task<Stream> RetrieveAsync(string storageName, Stream stream, CancellationToken cancellationToken);
}

public interface IImageStorageNameGenerator
{
    public string? Generate(Guid userId, Guid imageId, string mediaType);
}

public interface IImageUrlProvider
{
    public string Generate(string storageName);
}