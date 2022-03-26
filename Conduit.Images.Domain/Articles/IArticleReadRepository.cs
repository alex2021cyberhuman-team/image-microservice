﻿namespace Conduit.Images.Domain.Articles;

public interface IArticleReadRepository
{
    Task<ArticleDomainModel?> FindAsync(
        string articleSlug,
        CancellationToken cancellationToken);
}
