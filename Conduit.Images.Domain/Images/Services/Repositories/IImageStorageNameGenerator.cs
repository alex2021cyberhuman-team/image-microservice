namespace Conduit.Images.Domain.Images.Services.Repositories;

public interface IImageStorageNameGenerator
{
    public string Generate(Guid userId, Guid imageId, string mediaType);

    public bool TryGetMediaType(string storageName, out string? mediaType);
}
