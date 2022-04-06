using System.Net;
using Conduit.Images.Domain.Configuration;
using Conduit.Images.Domain.Images.RemoveArticleImage;
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
    [Produces(typeof(UploadArticleImageResponse.Model))]
    public async Task<IActionResult> Upload(
        [FromServices] IUploadArticleImageRequestHandler uploadArticleImageRequestHandler,
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
        var uploadArticleImageRequest = new UploadArticleImageRequest(
            streamProvider, HttpContext.Request.ContentType!, userId
        );
        var response = await uploadArticleImageRequestHandler.UploadAsync(uploadArticleImageRequest, cancellationToken);
        var actionResult = response.Error.GetAndLogActionResult(response.Data, null, _logger);
        return actionResult;
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Remove(Guid id,
        [FromServices] IRemoveArticleImageHandler removeArticleImageHandler,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetCurrentUserId();
        var removeArticleImageRequest = new RemoveArticleImageRequest(userId, id);
        var removeArticleImageResponse = await removeArticleImageHandler.RemoveAsync(removeArticleImageRequest, cancellationToken);
        var actionResult = removeArticleImageResponse.Error.GetAndLogActionResult(
            null,
            null,
            _logger);
        return actionResult;
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