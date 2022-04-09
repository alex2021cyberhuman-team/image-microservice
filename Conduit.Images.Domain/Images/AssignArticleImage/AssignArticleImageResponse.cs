using Conduit.Shared.ResponsesExtensions;

namespace Conduit.Images.Domain.Images.AssignArticleImage;

public class AssignArticleImageResponse : BaseResponse
{
    public AssignArticleImageResponse(Error error)
    {
        Error = error;
    }
}
