using Conduit.Images.Domain.Configuration;
using Conduit.Images.Domain.Images.Services.Repositories;
using Microsoft.Extensions.Options;

namespace Conduit.Images.DataAccess.Images.Services;

public class ConfiguredImageStorageNameGenerator : IImageStorageNameGenerator
{
    private readonly IOptionsMonitor<Options> _optionsMonitor;

    private readonly IOptionsMonitor<ImageConfiguration> _imageConfigurationsMonitor;


    public ConfiguredImageStorageNameGenerator(
        IOptionsMonitor<Options> optionsMonitor,
        IOptionsMonitor<ImageConfiguration> imageConfigurationsMonitor)
    {
        _optionsMonitor = optionsMonitor;
        _imageConfigurationsMonitor = imageConfigurationsMonitor;
    }

    private Options OptionsInstance => _optionsMonitor.CurrentValue;

    private ImageConfiguration ImageConfigurationsInstance => _imageConfigurationsMonitor.CurrentValue;

    public string Generate(Guid userId, Guid imageId, string mediaType)
    {
        var mediaTypeMapping = ImageConfigurationsInstance.MediaTypeMapping;
        var extension = mediaTypeMapping[mediaType];
        var storageNameFormat = OptionsInstance.StorageNameFormat;
        var storageName = string.Format(storageNameFormat, userId, imageId, extension);
        return storageName;
    }

    public bool TryGetMediaType(string storageName, out string? mediaType)
    {
        mediaType = string.Empty;
        var storageNameSplit = storageName.Split('.');
        if (storageNameSplit.Length < 2)
        {
            return false;
        }
        var extension = storageNameSplit[^1];
        var mediaTypeMapping = ImageConfigurationsInstance.ReverseMediaTypeMapping;
        var containsMediaType = mediaTypeMapping.TryGetValue(extension, out mediaType);
        return containsMediaType;
    }

    public class Options
    {
        public string StorageNameFormat { get; set; } = "u-{0}-i-{1}{2}";
    }
}
