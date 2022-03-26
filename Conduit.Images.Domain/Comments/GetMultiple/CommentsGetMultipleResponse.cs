using Conduit.Images.Domain.Comments.Models;

namespace Conduit.Images.Domain.Comments.GetMultiple;

public class CommentsGetMultipleResponse : BaseResponse
{
    public CommentsGetMultipleResponse(
        Error error)
    {
        Error = error;
    }

    public CommentsGetMultipleResponse(
        List<CommentOutputModel> items)
    {
        Output = new() { Comments = items };
    }

    public MultipleCommentsOutputModel Output { get; set; } = new();
}
