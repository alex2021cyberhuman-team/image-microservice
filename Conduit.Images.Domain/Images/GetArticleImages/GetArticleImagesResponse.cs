using Conduit.Images.Domain.Images.Common;
using Conduit.Shared.ResponsesExtensions;

namespace Conduit.Images.Domain.Images.GetArticleImages;

public class GetArticleImagesResponse : BaseResponse
{
    public GetArticleImagesResponse(List<ArticleImageResponseModel> images,
        Error error,
        string? errorDescription = null) : base(error, errorDescription)
    {
        Body = new(images);
    }

    public ResponseBody Body { get; set; }

    public class ResponseBody
    {
        public ResponseBody(List<ArticleImageResponseModel> images)
        {
            Images = images;
        }
        
        public List<ArticleImageResponseModel> Images { get; set; }
    }
}
