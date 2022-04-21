namespace Conduit.Images.Domain.Images.GetArticleImages;

public interface IGetArticleImagesHandler
{
    Task<GetArticleImagesResponse> GetAsync(GetArticleImagesRequest request, CancellationToken cancellationToken);
}
