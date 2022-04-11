using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Images.Domain.Images.RemoveUnassignedImages;

public interface IRemoveUnassignedImagesHandler
{
    Task RemoveAsync(CancellationToken cancellationToken = default);
}

