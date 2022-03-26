using Conduit.Shared.Validation;

namespace Conduit.Images.Domain.Comments.Create;

public interface ICommentCreateInputModelValidator
{
    Task<Validation> ValidateAsync(
        CommentCreateInputModel commentCreateInputModel);
}
