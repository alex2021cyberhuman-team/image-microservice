using Conduit.Images.Domain.Comments.Models;

namespace Conduit.Images.Domain.Comments.Repositories;

public interface ICommentsWriteRepository
{
    Task CreateAsync(
        CommentDomainModel commentDomainModel);

    Task DeleteAsync(
        Guid commentId);
}
