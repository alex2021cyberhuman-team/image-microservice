namespace Conduit.Images.Domain.Images.Services.Repositories;

public interface IImageStorage
{
    public Task RemoveAsync(string storageName, CancellationToken cancellationToken);

    public Task StoreAsync(string storageName, Stream stream, CancellationToken cancellationToken);

    public Task<Stream?> RetrieveAsync(string storageName, CancellationToken cancellationToken);
}
