namespace Conduit.Images.Domain.Comments.Delete;

public class CommentDeleteResponse : BaseResponse
{
    public CommentDeleteResponse(
        Error error)
    {
        Error = error;
    }

    public CommentDeleteResponse()
    {
    }
}
