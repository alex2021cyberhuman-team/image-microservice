using Conduit.Images.Domain.Comments.Models;

namespace Conduit.Images.Domain.Comments.Repositories;

public interface ICommentsReadRepository
{
    Task<CommentDomainModel?> FindAsync(
        Guid commentId,
        CancellationToken cancellationToken);

    Task<List<CommentOutputModel>> GetMultipleAsync(
        string articleSlug,
        Guid? userId,
        CancellationToken cancellationToken);
}
