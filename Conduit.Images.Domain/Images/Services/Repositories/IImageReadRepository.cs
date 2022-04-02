using Conduit.Images.Domain.Images.Common;

namespace Conduit.Images.Domain.Images.Services.Repositories;

public interface IImageReadRepository
{
    public Task<ArticleImageDomainModel?> FindAsync(Guid id, CancellationToken cancellationToken = default);
}
