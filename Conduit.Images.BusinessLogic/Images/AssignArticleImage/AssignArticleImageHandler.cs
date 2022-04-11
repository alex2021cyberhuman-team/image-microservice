using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Images.Domain.Articles;
using Conduit.Images.Domain.Images.AssignArticleImage;
using Conduit.Images.Domain.Images.Services.Repositories;
using Conduit.Shared.ResponsesExtensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Images.BusinessLogic.Images.AssignArticleImage;

public class AssignArticleImageHandler : IAssignArticleImageHandler
{
    private readonly IImageReadRepository _imageReadRepository;

    private readonly IImageWriteRepository _imageWriteRepository;

    private readonly IArticleReadRepository _articleReadRepository;

    private readonly ILogger _logger;

    public AssignArticleImageHandler(
        IImageReadRepository imageReadRepository,
        IImageWriteRepository imageWriteRepository,
        IArticleReadRepository articleReadRepository,
        ILogger<AssignArticleImageHandler> logger)
    {
        _imageReadRepository = imageReadRepository;
        _imageWriteRepository = imageWriteRepository;
        _articleReadRepository = articleReadRepository;
        _logger = logger;
    }


    public async Task<AssignArticleImageResponse> AssignAsync(AssignArticleImageRequest assignArticleImageRequest, CancellationToken cancellationToken = default)
    {
        var articleId = assignArticleImageRequest.ArticleId;
        var imageIds = assignArticleImageRequest.Body.ImagesToAssign;
        var userId = assignArticleImageRequest.UserId;
        _logger.LogInformation("Begin AsssignArticleImage {ArticleId} {UserId} {ImageIds}", articleId, userId, imageIds);

        var article = await _articleReadRepository.FindAsync(articleId, cancellationToken);

        if (article is null)
        {
            _logger.LogWarning("End AsssignArticleImage article not found {ArticleId}", articleId);
            return new(Error.NotFound);
        }

        if (article.AuthorId != userId)
        {
            _logger.LogWarning("End AsssignArticleImage {UserId} user is not an author of article {ArticleId}", userId, articleId);
            return new(Error.Forbidden);
        }

        var images = await _imageReadRepository.FindByIdsAsync(imageIds, cancellationToken);

        if (images.Count != imageIds.Count)
        {
            _logger.LogWarning("End AsssignArticleImage images not found");
            return new(Error.NotFound);
        }

        if (images.Any(x => x.UserId != userId))
        {
            _logger.LogWarning("End AsssignArticleImage images not uploaded by user {UserId}", userId);
            return new(Error.Forbidden);
        }

        var assignableImageIds = images
            .Where(x => x.ArticleId != articleId)
            .Select(x => x.Id).ToHashSet();
        await _imageWriteRepository.AssignAsync(
            article,
            assignableImageIds,
            CancellationToken.None);
        _logger.LogInformation("End AssignArticleImage successful");
        return new(Error.None);
    }
}
