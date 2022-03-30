using System.Net;
using Conduit.Images.Domain.Images.UploadArticleImage;
using Conduit.Images.WebApi.Configuration;
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

    private bool CheckContentType()
    {
        return HttpContext.Request.ContentType?.StartsWith("image/") ?? false;
    }

    private bool CheckContentLength()
    {
        return HttpContext.Request.ContentLength != 0 && (
                    _imageConfiguration.MaxImageSize == long.MaxValue ||
                    HttpContext.Request.ContentLength <= _imageConfiguration.MaxImageSize);
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