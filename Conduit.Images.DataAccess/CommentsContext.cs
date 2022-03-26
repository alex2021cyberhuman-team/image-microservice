using System.Reflection;
using Conduit.Images.DataAccess.Articles;
using Conduit.Images.DataAccess.Authors;
using Conduit.Images.DataAccess.Comments;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Images.DataAccess;

public class CommentsContext : DbContext
{
    protected CommentsContext()
    {
    }

    public CommentsContext(
        DbContextOptions<CommentsContext> options) : base(options)
    {
    }

    public DbSet<ArticleDbModel> Article => Set<ArticleDbModel>();

    public DbSet<AuthorDbModel> Author => Set<AuthorDbModel>();

    public DbSet<CommentDbModel> Comment => Set<CommentDbModel>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }
}
