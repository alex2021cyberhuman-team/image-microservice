using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Images.DataAccess.Extensions;

public static class DataAccessLayerInitializerExtensions
{
    public static async Task InitializeDataAccessLayerAsync(this IServiceScope scope)
    {
        var services = scope.ServiceProvider;
        var initializer = services.GetRequiredService<DataAccessLayerInitializer>();
        await initializer.InitializeAsync();
    }
}
