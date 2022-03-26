using Conduit.Images.Domain.Comments.GetMultiple;
using Conduit.Images.Domain.Comments.Repositories;

namespace Conduit.Images.BusinessLogic.Comments.GetMultiple;

public class CommentsGetMultipleHandler : ICommentsGetMultipleHandler
{
    private readonly ICommentsReadRepository _commentsReadRepository;

    public CommentsGetMultipleHandler(
        ICommentsReadRepository commentsReadRepository)
    {
        _commentsReadRepository = commentsReadRepository;
    }

    public async Task<CommentsGetMultipleResponse> HandleAsync(
        CommentsGetMultipleRequest request,
        CancellationToken cancellationToken)
    {
        var items =
            await _commentsReadRepository.GetMultipleAsync(request.ArticleSlug,
                request.UserId, cancellationToken);
        return new(items);
    }
}
