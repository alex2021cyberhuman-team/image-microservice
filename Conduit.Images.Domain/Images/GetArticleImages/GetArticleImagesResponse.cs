using Conduit.Images.Domain.Images.Common;
using Conduit.Shared.ResponsesExtensions;

namespace Conduit.Images.Domain.Images.GetArticleImages;

public class GetArticleImagesResponse : BaseResponse
{
    public GetArticleImagesResponse(IEnumerable<ArticleImageResponseModel>? images,
        Error error,
        string? errorDescription = null) : base(error, errorDescription)
    {
        Body = images != null ? new(images) : null;
    }

    public ResponseBody? Body { get; set; }

    public class ResponseBody
    {
        public ResponseBody(IEnumerable<ArticleImageResponseModel> images)
        {
            Images = images;
        }
        
        public IEnumerable<ArticleImageResponseModel> Images { get; set; }
    }
}
