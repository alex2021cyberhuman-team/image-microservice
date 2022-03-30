using Conduit.Images.Domain.Articles;
using Conduit.Images.Domain.Images.Services.Repositories;
using Conduit.Images.Domain.Images.UploadArticleImage;
using Conduit.Shared.Events.Services;
using Conduit.Shared.Events.Models.Images;
using Conduit.Images.Domain;

namespace Conduit.Images.BusinessLogic.Images.UploadArticleImage;

public class UploadArticleImageRequestHandler : IUploadArticleImageRequestHandler
{
    private readonly IImageWriteRepository _imageWriteRepository;
    private readonly IImageStorageNameGenerator _imageStorageNameGenerator;
    private readonly IImageUrlProvider _imageUrlProvider;
    private readonly IEventProducer<UploadArticleImageEventModel> _eventProducer;

    public UploadArticleImageRequestHandler(
        IImageWriteRepository imageWriteRepository,
        IImageStorageNameGenerator imageStorageNameGenerator,
        IImageUrlProvider imageUrlProvider,
        IEventProducer<UploadArticleImageEventModel> eventProducer)
    {
        _imageWriteRepository = imageWriteRepository;
        _imageStorageNameGenerator = imageStorageNameGenerator;
        _imageUrlProvider = imageUrlProvider;
        _eventProducer = eventProducer;
    }

    public async Task<UploadArticleImageResponse> UploadAsync(UploadArticleImageRequest request, CancellationToken cancellationToken = default)
    {
        var imageId = Guid.NewGuid();
        var userId = request.UserId;
        var mediaType = request.ContentType;
        var uploaded = DateTime.UtcNow;
        var imageStorageName = _imageStorageNameGenerator.Generate(userId, imageId, mediaType);
        if (imageStorageName is null)
        {
            return new(Error.BadRequest);
        }
        var stream = await request.StreamProvider.ProvideStreamAsync();
        var imageUrl = _imageUrlProvider.Generate(imageStorageName);
        var domainModel = await _imageWriteRepository.SaveAsync(
            userId,
            stream,
            mediaType,
            imageId,
            imageStorageName,
            imageUrl,
            uploaded,
            cancellationToken
        );
        var uploadArticleImageResponse = new UploadArticleImageResponse(
            new UploadArticleImageResponse.Model(new(null, imageId, imageUrl, mediaType))
        );
        return uploadArticleImageResponse;
    }
}
