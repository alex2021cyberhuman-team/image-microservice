using Conduit.Images.BusinessLogic.Images.Shared;
using Conduit.Images.Domain.Articles;
using Conduit.Images.Domain.Images.Common;
using Conduit.Images.Domain.Images.GetArticleImages;
using Conduit.Images.Domain.Images.Services.Repositories;
using Conduit.Shared.ResponsesExtensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Conduit.Images.BusinessLogic.Images.GetArticleImages;

class GetArticleImagesHandler : IGetArticleImagesHandler
{
    private readonly IImageReadRepository _imageReadRepository;
    
    private readonly IArticleReadRepository _articleReadRepository;

    private readonly ILogger _logger;

    private readonly IStringLocalizer _stringLocalizer;

    public GetArticleImagesHandler(
        IImageReadRepository imageReadRepository,
        IArticleReadRepository articleReadRepository,
        ILogger<GetArticleImagesHandler> logger,
        IStringLocalizer stringLocalizer)
    {
        _imageReadRepository = imageReadRepository;
        _articleReadRepository = articleReadRepository;
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<GetArticleImagesResponse> GetAsync(GetArticleImagesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Begin GetArticleImages {Request}", request);      
        var articleId = request.ArticleId;
        var article = await _articleReadRepository.FindAsync(articleId, cancellationToken);
        
        if (article is null)
        {
            _logger.LogWarning("End GetArticleImages article not found");
            var errorDescription = _stringLocalizer[LocalizationKeys.ArticleNotFound];
            return new(null, Error.NotFound, errorDescription);        
        }

        if (article.AuthorId != request.UserId)
        {
            _logger.LogWarning("End GetArticleImages user is not an author of article");
            var errorDescription = _stringLocalizer[LocalizationKeys.UserNotAuthor];
            return new(null, Error.NotFound, errorDescription);
        }

        var images = await _imageReadRepository.FindByArticleIdAsync(articleId, cancellationToken);
        var models = images.Cast<ArticleImageResponseModel>();
        _logger.LogInformation("End GetArticleImages successful");
        return new GetArticleImagesResponse(models, Error.None);
    }
}
