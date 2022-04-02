using Conduit.Images.Domain.Images.Services.Repositories;
using Microsoft.Extensions.Options;

namespace Conduit.Images.DataAccess.Images.Services;

public class ConfiguredImageUrlProvider : IImageUrlProvider
{
    public IOptionsMonitor<Options> _optionsMonitor;

    public ConfiguredImageUrlProvider(IOptionsMonitor<Options> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    public Options Current => _optionsMonitor.CurrentValue;

    public string Provide(string storageName)
    {
        var enpointToRetriveFormat = Current.EnpointToRetriveFormat;
        var url = string.Format(enpointToRetriveFormat, storageName);
        return url;
    }

    public class Options
    {
        public string EnpointToRetriveFormat { get; set; } = "http://localhost/images/{0}";
    }
}
