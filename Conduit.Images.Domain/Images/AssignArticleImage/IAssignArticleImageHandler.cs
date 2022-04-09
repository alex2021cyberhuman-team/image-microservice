using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Images.Domain.Images.AssignArticleImage;

public interface IAssignArticleImageHandler
{
    Task<AssignArticleImageResponse> AssignAsync(AssignArticleImageRequest assignArticleImageRequest, CancellationToken cancellationToken = default);
}
