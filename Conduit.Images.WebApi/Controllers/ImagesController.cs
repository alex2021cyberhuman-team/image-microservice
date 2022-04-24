using System.Net;
using Conduit.Images.Domain.Configuration;
using Conduit.Images.Domain.Images.AssignArticleImage;
using Conduit.Images.Domain.Images.GetArticleImages;
using Conduit.Images.Domain.Images.RemoveArticleImage;
using Conduit.Images.Domain.Images.Services.Repositories;
using Conduit.Images.Domain.Images.UploadArticleImage;
using Conduit.Images.WebApi.Services;
using Conduit.Shared.AuthorizationExtensions;
using Conduit.Shared.ResponsesExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Conduit.Images.WebApi.Controllers.ImagesController;

[ApiController]
[Route("images")]
public class ImagesController : ControllerBase
{
    private readonly IStringLocalizer _stringLocalizer;
    private readonly ImageConfiguration _imageConfiguration;
    private readonly ILogger _logger;

    public ImagesController(
        IStringLocalizer stringLocalizer,
        IOptions<ImageConfiguration> imageConfigration,
        ILogger<ImagesController> logger)
    {
        _stringLocalizer = stringLocalizer;
        _logger = logger;
        _imageConfiguration = imageConfigration.Value;
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(UploadArticleImageResponse.Model), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
    public async Task<IActionResult> Upload(
        [FromServices] IUploadArticleImageRequestHandler handler,
        CancellationToken cancellationToken)
    {
        if (!CheckContentLength())
        {
            return InvalidContentLength;
        }

        if (!CheckContentType())
        {
            return InvalidContentType;
        }

        var userId = HttpContext.GetCurrentUserId();
        var streamProvider = new ContentBodyRequestStreamProvider(HttpContext);
        var request = new UploadArticleImageRequest(
            streamProvider, HttpContext.Request.ContentType!, userId);
        var response = await handler.UploadAsync(request, cancellationToken);
        var actionResult = response.Error.GetAndLogActionResult(response.Data, null, _logger, response.ErrorDescription);
        return actionResult;
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Remove(Guid id,
        [FromServices] IRemoveArticleImageHandler handler,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();
        var request = new RemoveArticleImageRequest(userId, id);
        var response = await handler.RemoveAsync(request, cancellationToken);
        var actionResult = response.Error.GetAndLogActionResult(
            null,
            null,
            _logger,
            response.ErrorDescription);
        return actionResult;
    }

    [Authorize]
    [HttpPost("assign/{articleId:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Assign(Guid articleId,
            [FromBody] AssignArticleImageRequest.RequestBody body,
            [FromServices] IAssignArticleImageHandler handler,
            CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();
        var request = new AssignArticleImageRequest(userId, articleId, body);
        var response = await handler.AssignAsync(request, cancellationToken);
        var actionResult = response.Error.GetAndLogActionResult(
            null,
            null,
            _logger,
            response.ErrorDescription);
        return actionResult;
    }

    [HttpGet("article/{articleId}")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetArticleImages(Guid articleId, 
        [FromServices] IGetArticleImagesHandler handler,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();
        var request = new GetArticleImagesRequest(userId, articleId);
        var response = await handler.GetAsync(request, cancellationToken);
        var actionResult = response.Error.GetAndLogActionResult(
            response.Body,
            null,
            _logger,
            response.ErrorDescription);
        return actionResult;
    }

    [HttpGet("{storageName}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Retrieve(string storageName,
        [FromServices] IImageStorage imageStorage,
        [FromServices] IImageStorageNameGenerator imageStorageNameGenerator,
        CancellationToken cancellationToken)
    {        
        _logger.LogInformation("Begin RetrieveArticleImage {StorageName}", storageName);
        if (!imageStorageNameGenerator.TryGetMediaType(storageName, out var mediaType))
        {
            _logger.LogWarning("End RetrieveArticleImage unable to determine media type");
            return NotFound();
        }
        var stream = await imageStorage.RetrieveAsync(storageName, cancellationToken);
        if (stream != null)
        {
            _logger.LogInformation("End RetrieveArticleImage successful");
            return File(stream, mediaType!);
        }
        _logger.LogWarning("End RetrieveArticleImage unable to find stream");
        return NotFound();
    }

    private bool CheckContentType()
    {
        var contentType = HttpContext.Request.ContentType ?? string.Empty;
        var mediaTypeMapping = _imageConfiguration.MediaTypeMapping;
        var validMediaType = mediaTypeMapping.ContainsKey(contentType);
        return validMediaType;
    }

    private bool CheckContentLength()
    {
        var contentLength = HttpContext.Request.ContentLength;
        var notEqualZero = contentLength != 0;
        var lowerThanMaxImageSize = _imageConfiguration.MaxImageSize == long.MaxValue || HttpContext.Request.ContentLength <= _imageConfiguration.MaxImageSize;
        var validContentLength = notEqualZero && lowerThanMaxImageSize;
        return validContentLength;
    }

    private IActionResult InvalidContentLength => new ObjectResult(new
    {
        errors = new Dictionary<string, IEnumerable<string>>
        {
            ["image"] = new string[] { _stringLocalizer.GetString("InvalidContentLength") }
        }
    })
    {
        StatusCode = (int)HttpStatusCode.UnprocessableEntity
    };

    private IActionResult InvalidContentType => new ObjectResult(new
    {
        errors = new Dictionary<string, IEnumerable<string>>
        {
            ["image"] = new string[] { _stringLocalizer.GetString("InvalidContentType") }
        }
    })
    {
        StatusCode = (int)HttpStatusCode.UnprocessableEntity
    };
}
