using System.ComponentModel.DataAnnotations;
using Conduit.Images.Domain.Authors;

namespace Conduit.Images.Domain.Comments.Models;

public class CommentOutputModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public string Body { get; set; } = string.Empty;

    [Required]
    public AuthorOutputModel Author { get; set; } = new();
}
