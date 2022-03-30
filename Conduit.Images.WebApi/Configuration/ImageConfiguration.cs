namespace Conduit.Images.WebApi.Configuration;

public class ImageConfiguration
{
    /// <summary>
    ///     Max image size
    /// </summary>
    /// <value>bytes</value>
    public long MaxImageSize { get; set; } = 1_000_000;
}