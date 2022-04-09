using Conduit.Images.Domain.Images.Common;

namespace Conduit.Images.Domain.Images.Services.Repositories;

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

    public Task AssignAsync
}
