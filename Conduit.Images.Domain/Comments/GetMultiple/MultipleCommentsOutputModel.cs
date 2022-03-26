using System.ComponentModel.DataAnnotations;
using Conduit.Images.Domain.Comments.Models;

namespace Conduit.Images.Domain.Comments.GetMultiple;

public class MultipleCommentsOutputModel
{
    [Required]
    public List<CommentOutputModel> Comments { get; set; } = new();
}
