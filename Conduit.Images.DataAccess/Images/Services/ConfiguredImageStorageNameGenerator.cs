using Conduit.Images.Domain.Images.Services.Repositories;
using Microsoft.Extensions.Options;

namespace Conduit.Images.DataAccess.Images.Services;

public class ConfiguredImageStorageNameGenerator : IImageStorageNameGenerator
{
    public IOptionsMonitor<Options> _optionsMonitor;

    public ConfiguredImageStorageNameGenerator(IOptionsMonitor<Options> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    public Options Current => _optionsMonitor.CurrentValue;

    public string Generate(Guid userId, Guid imageId, string mediaType)
    {
        var mediaTypeMapping = Current.MediaTypeMapping;
        var extension = mediaTypeMapping[mediaType];
        var storageNameFormat = Current.StorageNameFormat;
        var storageName = string.Format(storageNameFormat, userId, imageId, extension);
        return storageName;
    }

    public class Options
    {
        public string StorageNameFormat { get; set; } = "usercontent-{0}-image-{1}.{2}";

        public Dictionary<string, string> MediaTypeMapping { get; set; } = new()
        {
            ["image/apng"] = ".apng",
            ["image/avif"] = ".avif",
            ["image/gif"] = ".gif",
            ["image/jpeg"] = ".jpg",
            ["image/png"] = ".png",
            ["image/svg+xml"] = ".svg",
            ["image/webp"] = ".webp",
            ["image/bmp"] = ".bmp",
            ["image/x-icon"] = ".ico",
            ["image/tiff"] = ".tif"
        };
    }
}
