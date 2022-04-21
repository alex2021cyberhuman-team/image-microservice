namespace Conduit.Images.Domain.Images.Common;

public abstract class ImageDomainModel
{
    public ImageDomainModel(
        Guid id,
        Guid userId,
        string url,
        string storageName,
        string mediaType,
        DateTime uploaded)
    {
        Id = id;
        Url = url;
        StorageName = storageName;
        MediaType = mediaType;
        Uploaded = uploaded;
        UserId = userId;
    }

    public Guid Id { get; }

    public Guid UserId { get; set; }

    public string Url { get; }

    public string StorageName { get; set; }

    public string MediaType { get; }

    public DateTime Uploaded { get; set; }
}
